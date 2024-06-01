using Microsoft.AspNetCore.Mvc;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.ViewModels;
using System.Net;

namespace PizzeriaRestApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ClientController : Controller
	{
		private readonly ILogger _logger;

		private readonly IClientLogic _logic;

		private readonly IMessageInfoLogic _mailLogic;

		public ClientController(IClientLogic logic, ILogger<ClientController> logger, IMessageInfoLogic mailLogic)
		{
			_logger = logger;
			_logic = logic;
			_mailLogic = mailLogic;
		}

		[HttpGet]
		public ClientViewModel? Login(string login, string password)
		{
			try
			{
				return _logic.ReadElement(new ClientSearchModel
				{
					Email = login,
					Password = password
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка входа в систему");
				throw;
			}
		}

		[HttpPost]
		public void Register(ClientBindingModel model)
		{
			try
			{
				_logic.Create(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка регистрации");
				Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
			}
		}

		[HttpPost]
		public void UpdateData(ClientBindingModel model)
		{
			try
			{
				_logic.Update(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка обновления данных");
				throw;
			}
		}

        [HttpGet]
        public List<MessageInfoViewModel>? GetMessages(int clientId, int page, int pagesize = 1)
        {
            try
            {
                return _mailLogic.ReadList(new MessageInfoSearchModel
                {
                    ClientId = clientId,
                    PageLength = pagesize,
                    PageIndex = page
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения писем клиента");
                throw;
            }
        }
    }
}
