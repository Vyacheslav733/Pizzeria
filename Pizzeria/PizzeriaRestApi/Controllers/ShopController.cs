using Microsoft.AspNetCore.Mvc;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.ViewModels;

namespace PizzeriaRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShopController : Controller
    {
        private readonly ILogger _logger;
        private readonly IShopLogic _shopLogic;

        public ShopController(ILogger<ShopController> logger, IShopLogic shopLogic)
        {
            _logger = logger;
            _shopLogic = shopLogic;
        }

        [HttpGet]
        public bool Authentication(string password)
        {
            return CheckPassword(password);
        }

        [HttpGet]
        public List<ShopViewModel>? GetShopList(string password)
        {
            if (!CheckPassword(password))
            {
                return null;
            }
            try
            {
                return _shopLogic.ReadList(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка магазинов");
                throw;
            }
        }

        [HttpGet]
        public ShopPizzaViewModel? GetShop(int shopId, string password)
        {
            if (!CheckPassword(password))
            {
                return null;
            }
            try
            {
                var shop = _shopLogic.ReadElement(new ShopSearchModel { Id = shopId });
                return new ShopPizzaViewModel
                {
                    Shop = shop,
                    ShopPizza = shop.ShopPizzas.ToDictionary(x => x.Key, x => new PizzaCount
                    {
                        Pizza = new PizzaViewModel()
                        {
                            Id = x.Value.Item1.Id,
                            PizzaName = x.Value.Item1.PizzaName,
                            PizzaComponents = x.Value.Item1.PizzaComponents,
                            Price = x.Value.Item1.Price,
                        },
                        Count = x.Value.Item2
                    })
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения магазина");
                throw;
            }
        }

        [HttpPost]
        public void CreateShop(ShopBindingModel model, string password)
        {
            if (!CheckPassword(password))
            {
                return;
            }
            try
            {
                _shopLogic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания магазина");
                throw;
            }
        }

        [HttpPost]
        public void UpdateShop(ShopBindingModel model, string password)
        {
            if (!CheckPassword(password))
            {
                return;
            }
            try
            {
                _shopLogic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления магазина");
                throw;
            }
        }

        [HttpDelete]
        public void DeleteShop(int shopId, string password)
        {
            if (!CheckPassword(password))
            {
                return;
            }
            try
            {
                _shopLogic.Delete(new ShopBindingModel { Id = shopId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления магазина");
                throw;
            }
        }

        [HttpPost]
        public void MakeSypply(SupplyBindingModel model, string password)
        {
            if (!CheckPassword(password))
            {
                return;
            }
            try
            {
                _shopLogic.MakeSupply(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания поставки в магазин");
                throw;
            }
        }

        private bool CheckPassword(string password)
        {
            return APIConfig.ShopPassword == password;
        }
    }
}
