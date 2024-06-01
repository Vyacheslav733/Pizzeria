using Microsoft.AspNetCore.Mvc;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaShopApp.Models;
using System.Diagnostics;

namespace PizzeriaShopApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (APIClient.Password == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<ShopViewModel>>($"api/shop/getshoplist?password={APIClient.Password}"));
        }

        [HttpGet]
        public IActionResult Enter()
        {
            return View();
        }

        [HttpPost]
        public void Enter(string password)
        {
            bool resout = APIClient.GetRequest<bool>($"/api/shop/authentication?password={password}");
            if (!resout)
            {
                Response.Redirect("../Home/Enter");
                return;
            }
            APIClient.Password = password;
            Response.Redirect("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (APIClient.Password == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View("Shop");
        }

        [HttpPost]
        public void Create(int id, string shopname, string adress, DateTime openingdate, int maxcount)
        {
            if (string.IsNullOrEmpty(shopname) || string.IsNullOrEmpty(adress))
            {
                throw new Exception("Название или адрес не может быть пустым");
            }
            if (openingdate == default(DateTime))
            {
                throw new Exception("Дата открытия не может быть пустой");
            }

            APIClient.PostRequest($"api/shop/createshop?password={APIClient.Password}", new ShopBindingModel
            {
                Id = id,
                ShopName = shopname,
                Adress = adress,
                OpeningDate = openingdate,
                PizzaMaxCount = maxcount
            });
            Response.Redirect("Index");
        }

        [HttpGet]
        public IActionResult Update(int Id)
        {
            if (APIClient.Password == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View("Shop", APIClient.GetRequest<ShopPizzaViewModel>($"api/shop/getshop?shopId={Id}&password={APIClient.Password}"));
        }

        [HttpPost]
        public void Update(int id, string shopname, string adress, DateTime openingdate, int maxcount)
        {
            if (string.IsNullOrEmpty(shopname) || string.IsNullOrEmpty(adress))
            {
                throw new Exception("Название или адрес не может быть пустым");
            }
            if (openingdate == default(DateTime))
            {
                throw new Exception("Дата открытия не может быть пустой");
            }
            APIClient.PostRequest($"api/shop/updateshop?password={APIClient.Password}", new ShopBindingModel
            {
                Id = id,
                ShopName = shopname,
                Adress = adress,
                OpeningDate = openingdate,
                PizzaMaxCount = maxcount
            });
            Response.Redirect("../Index");
        }

        [HttpPost]
        public void Delete(int Id)
        {
            APIClient.DeleteRequest($"api/shop/deleteshop?shopId={Id}&password={APIClient.Password}");
            Response.Redirect("../Index");
        }

        [HttpGet]
        public IActionResult Supply()
        {
            if (APIClient.Password == null)
            {
                return Redirect("~/Home/Enter");
            }

            ViewBag.Shops = APIClient.GetRequest<List<ShopViewModel>>($"api/shop/getshoplist?password={APIClient.Password}");
            ViewBag.Pizzas = APIClient.GetRequest<List<PizzaViewModel>>($"api/main/getpizzalist");
            return View();
        }

        [HttpPost]
        public void Supply(int shop, int pizza, int count)
        {
            APIClient.PostRequest($"api/shop/makesypply?password={APIClient.Password}", new SupplyBindingModel
            {
                ShopId = shop,
                PizzaId = pizza,
                Count = count
            });
            Response.Redirect("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
