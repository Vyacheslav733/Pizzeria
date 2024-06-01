using Microsoft.Extensions.Logging;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaBusinessLogic.BusinessLogics
{
    public class ShopLogic : IShopLogic
    {
        private readonly ILogger _logger;
        private readonly IShopStorage _shopStorage;
        private readonly IPizzaStorage _pizzaStorage;

        public ShopLogic(ILogger<ShopLogic> logger, IShopStorage shopStorage, IPizzaStorage pizzaStorage)
        {
            _logger = logger;
            _shopStorage = shopStorage;
            _pizzaStorage = pizzaStorage;
        }

        public List<ShopViewModel>? ReadList(ShopSearchModel? model)
        {
            _logger.LogInformation("ReadList. ShopName:{ShopName}.Id:{ Id}", model?.ShopName, model?.Id);
            var list = model == null ? _shopStorage.GetFullList() : _shopStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public ShopViewModel? ReadElement(ShopSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. ShopName:{ShopName}.Id:{ Id}", model.ShopName, model.Id);
            var element = _shopStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public bool Create(ShopBindingModel model)
        {
            CheckModel(model);
            if (_shopStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Update(ShopBindingModel model)
        {
            CheckModel(model);
            if (_shopStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(ShopBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_shopStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        public bool MakeSupply(SupplyBindingModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (model.Count <= 0)
            {
                throw new ArgumentException("Количество пиццы должно быть больше 0");
            }
            var shop = _shopStorage.GetElement(new ShopSearchModel
            {
                Id = model.ShopId
            });
            if (shop == null)
            {
                throw new ArgumentException("Магазина не существует");
            }
            if (shop.ShopPizzas.ContainsKey(model.PizzaId))
            {
                var oldValue = shop.ShopPizzas[model.PizzaId];
                oldValue.Item2 += model.Count;
                shop.ShopPizzas[model.PizzaId] = oldValue;
            }
            else
            {
                var pizza = _pizzaStorage.GetElement(new PizzaSearchModel
                {
                    Id = model.PizzaId
                });
                if (pizza == null)
                {
                    throw new ArgumentException($"Поставка: Товар с id:{model.PizzaId} не найденн");
                }
                shop.ShopPizzas.Add(model.PizzaId, (pizza, model.Count));
            }

            _shopStorage.Update(new ShopBindingModel()
            {
                Id = shop.Id,
                ShopName = shop.ShopName,
                Adress = shop.Adress,
                OpeningDate = shop.OpeningDate,
                ShopPizzas = shop.ShopPizzas,
                PizzaMaxCount = shop.PizzaMaxCount,
            });

            return true;
        }

        private void CheckModel(ShopBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.Adress))
            {
                throw new ArgumentException("Адрес магазина длжен быть заполнен", nameof(model.Adress));
            }
            if (string.IsNullOrEmpty(model.ShopName))
            {
                throw new ArgumentException("Название магазина должно быть заполнено", nameof(model.ShopName));
            }
            _logger.LogInformation("Shop. ShopName:{ShopName}.Adres:{Adres}.OpeningDate:{OpeningDate}.Id:{ Id}", model.ShopName, model.Adress, model.OpeningDate, model.Id);
            var element = _shopStorage.GetElement(new ShopSearchModel
            {
                ShopName = model.ShopName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Магазин с таким названием уже есть");
            }
        }

        public bool Sale(SupplySearchModel model)
        {
            if (!model.PizzaId.HasValue || !model.Count.HasValue)
            {
                return false;
            }
            _logger.LogInformation("Check pizza count in all shops");
            if (_shopStorage.Sale(model))
            {
                _logger.LogInformation("Selling sucsess");
                return true;
            }
            _logger.LogInformation("Selling failed");
            return false;
        }
    }
}
