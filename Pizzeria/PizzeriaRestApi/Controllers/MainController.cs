using Microsoft.AspNetCore.Mvc;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.ViewModels;

namespace PizzeriaRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : Controller
    {
        private readonly ILogger _logger;

        private readonly IOrderLogic _order;

        private readonly IPizzaLogic _pizza;

        public MainController(ILogger<MainController> logger, IOrderLogic order, IPizzaLogic pizza)
        {
            _logger = logger;
            _order = order;
            _pizza = pizza;
        }

        [HttpGet]
        public List<PizzaViewModel>? GetPizzaList()
        {
            try
            {
                return _pizza.ReadList(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка продуктов");
                throw;
            }
        }

        [HttpGet]
        public PizzaViewModel? GetPizza(int pizzaId)
        {
            try
            {
                return _pizza.ReadElement(new PizzaSearchModel { Id = pizzaId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения продукта по id={Id}", pizzaId);
                throw;
            }
        }

        [HttpGet]
        public List<OrderViewModel>? GetOrders(int clientId)
        {
            try
            {
                return _order.ReadList(new OrderSearchModel { ClientId = clientId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка заказов клиента id={Id}", clientId);
                throw;
            }
        }

        [HttpPost]
        public void CreateOrder(OrderBindingModel model)
        {
            try
            {
                _order.CreateOrder(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания заказа");
                throw;
            }
        }
    }
}
