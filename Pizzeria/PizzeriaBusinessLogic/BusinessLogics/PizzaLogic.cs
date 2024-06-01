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
    public class PizzaLogic : IPizzaLogic
    {
        private readonly ILogger _logger;
        private readonly IPizzaStorage _PizzaStorage;
        public PizzaLogic(ILogger<PizzaLogic> logger, IPizzaStorage PizzaStorage)
        {
            _logger = logger;
            _PizzaStorage = PizzaStorage;
        }
        public List<PizzaViewModel>? ReadList(PizzaSearchModel? model)
        {
            _logger.LogInformation("ReadList. PizzaName:{PizzaName}.Id:{ Id}", model?.PizzaName, model?.Id);
            var list = model == null ? _PizzaStorage.GetFullList() : _PizzaStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public PizzaViewModel? ReadElement(PizzaSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. PizzaName:{PizzaName}.Id:{ Id}", model.PizzaName, model.Id);
            var element = _PizzaStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }
        public bool Create(PizzaBindingModel model)
        {
            CheckModel(model);
            if (_PizzaStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        public bool Update(PizzaBindingModel model)
        {
            CheckModel(model);
            if (_PizzaStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
        public bool Delete(PizzaBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_PizzaStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }
        private void CheckModel(PizzaBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.PizzaName))
            {
                throw new ArgumentNullException("Нет названия пиццы", nameof(model.PizzaName));
            }
            if (model.Price <= 0)
            {
                throw new ArgumentNullException("Цена пиццы должна быть больше 0", nameof(model.Price));
            }
            if (model.PizzaComponents == null || model.PizzaComponents.Count == 0)
            {
                throw new ArgumentNullException("Перечень ингредиентов не может быть пустым", nameof(model.PizzaComponents));
            }
            _logger.LogInformation("Pizza. PizzaName:{PizzaName}.Price:{Price}.Id: { Id}", model.PizzaName, model.Price, model.Id);
            var element = _PizzaStorage.GetElement(new PizzaSearchModel
            {
                PizzaName = model.PizzaName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Пицца с таким названием уже есть");
            }
        }
    }
}
