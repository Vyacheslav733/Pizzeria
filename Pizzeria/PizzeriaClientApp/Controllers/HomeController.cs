using Microsoft.AspNetCore.Mvc;
using PizzeriaClientApp.Models;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using System.Diagnostics;

namespace PizzeriaClientApp.Controllers
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
			if (APIClient.Client == null)
			{
				return Redirect("~/Home/Enter");
			}
			return View(APIClient.GetRequest<List<OrderViewModel>>($"api/main/getorders?clientId={APIClient.Client.Id}"));
		}

		[HttpGet]
		public IActionResult Privacy()
		{
			if (APIClient.Client == null)
			{
				return Redirect("~/Home/Enter");
			}
			return View(APIClient.Client);
		}

		[HttpPost]
		public void Privacy(string login, string password, string fio)
		{
			if (APIClient.Client == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fio))
			{
				throw new Exception("Введите логин, пароль и ФИО");
			}
			APIClient.PostRequest("api/client/updatedata", new ClientBindingModel
			{
				Id = APIClient.Client.Id,
				ClientFIO = fio,
				Email = login,
				Password = password
			});

			APIClient.Client.ClientFIO = fio;
			APIClient.Client.Email = login;
			APIClient.Client.Password = password;
			Response.Redirect("Index");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public IActionResult Enter()
		{
			return View();
		}

		[HttpPost]
		public void Enter(string login, string password)
		{
			if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
			{
				throw new Exception("Введите логин и пароль");
			}
			APIClient.Client = APIClient.GetRequest<ClientViewModel>($"api/client/login?login={login}&password={password}");
			if (APIClient.Client == null)
			{
				throw new Exception("Неверный логин/пароль");
			}
			Response.Redirect("Index");
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public void Register(string login, string password, string fio)
		{
			if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fio))
			{
				throw new Exception("Введите логин, пароль и ФИО");
			}
			APIClient.PostRequest("api/client/register", new ClientBindingModel
			{
				ClientFIO = fio,
				Email = login,
				Password = password
			});
			Response.Redirect("Enter");
			return;
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Pizzas = APIClient.GetRequest<List<PizzaViewModel>>("api/main/getpizzalist");
			return View();
		}

		[HttpPost]
		public void Create(int pizza, int count)
		{
			if (APIClient.Client == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (count <= 0)
			{
				throw new Exception("Количество и сумма должны быть больше 0");
			}
			APIClient.PostRequest("api/main/createorder", new OrderBindingModel
			{
				ClientId = APIClient.Client.Id,
				PizzaId = pizza,
				Count = count,
				Sum = Calc(count, pizza)
			});
			Response.Redirect("Index");
		}

		[HttpPost]
		public double Calc(int count, int pizza)
		{
			var piz = APIClient.GetRequest<PizzaViewModel>($"api/main/getpizza?pizzaId={pizza}");
			return count * (piz?.Price ?? 1);
		}

		[HttpGet]
		public IActionResult Mails(int page = 1)
		{
			if (APIClient.Client == null)
			{
				return Redirect("~/Home/Enter");
			}

			page = Math.Max(page, 1);
			return View(APIClient.GetRequest<List<MessageInfoViewModel>>($"api/client/getmessages?clientId={APIClient.Client.Id}&page={page}"));
		}
	}
}
