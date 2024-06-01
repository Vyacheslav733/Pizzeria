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
    public class MessageInfoLogic : IMessageInfoLogic
    {
        private readonly ILogger _logger;
        private readonly IMessageInfoStorage _messageInfoStorage;
        private readonly IClientStorage _clientStorage;
        public MessageInfoLogic(ILogger<MessageInfoLogic> logger, IMessageInfoStorage messageInfoStorage, IClientStorage clientStorage)
        {
            _logger = logger;
            _messageInfoStorage = messageInfoStorage;
            _clientStorage = clientStorage;
        }
        public List<MessageInfoViewModel>? ReadList(MessageInfoSearchModel? model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadList. MessageId:{MessageId}.ClientId:{ClientId}.PageLength:{PageLength}.PageCount:{PageIndex}", model?.MessageId, model?.ClientId, model?.PageLength, model?.PageIndex);
            var list = _messageInfoStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }
        public bool Create(MessageInfoBindingModel model)
        {
            CheckModel(model);
            var message = _messageInfoStorage.Insert(model);
            if (message == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }
        private void CheckModel(MessageInfoBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (string.IsNullOrEmpty(model.MessageId))
            {
                throw new ArgumentNullException("Не указан id сообщения", nameof(model.MessageId));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.SenderName))
            {
                throw new ArgumentNullException("Не указао имя отправителя(электронная почта)", nameof(model.SenderName));
            }
            if (string.IsNullOrEmpty(model.Subject))
            {
                throw new ArgumentNullException("Не указана темма", nameof(model.Subject));
            }
            if (string.IsNullOrEmpty(model.Body))
            {
                throw new ArgumentNullException("Не указан текст сообщения", nameof(model.Subject));
            }
            _logger.LogInformation("MessageInfo. MessageId:{MessageId}.SenderName:{SenderName}.Subject:{Subject}.Body:{Body}", model.MessageId, model.SenderName, model.Subject, model.Body);
            var element = _clientStorage.GetElement(new ClientSearchModel
            {
                Email = model.SenderName
            });
            if (element == null)
            {
                _logger.LogWarning("Не удалоссь найти клиента, отправившего письмо с адреса Email:{Email}", model.SenderName);
            }
            else
            {
                model.ClientId = element.Id;
            }
        }

        public MessageInfoViewModel? ReadElement(MessageInfoSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. MessageId:{MessageId}", model?.MessageId);
            var element = _messageInfoStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.MessageId);
            return element;
        }

        public bool Update(MessageInfoBindingModel model)
        {
            CheckModel(model, withParams: false);
            if (_messageInfoStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
    }
}