using PizzeriaDataModels.Enums;
using Microsoft.Extensions.Logging;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using MigraDoc.Rendering;
using PizzeriaBusinessLogic.MailWorker;

namespace PizzeriaBusinessLogic.BusinessLogics
{
    public class OrderLogic : IOrderLogic
    {
        private readonly ILogger _logger;
        private readonly IOrderStorage _orderStorage;
        private readonly AbstractMailWorker _mailWorker;
        static readonly object _locker = new object();
        private readonly IShopStorage _shopStorage;
        public OrderLogic(ILogger<OrderLogic> logger, IOrderStorage orderStorage, IShopStorage shopStorage, AbstractMailWorker mailWorker)
        {
            _logger = logger;
            _orderStorage = orderStorage;
            _shopStorage = shopStorage;
            _mailWorker = mailWorker;
        }

        public OrderViewModel? ReadElement(OrderSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. ClientId:{ClientId}.Status:{Status}.ImplementerId:{ImplementerId}.DateFrom:{DateFrom}.DateTo:{DateTo}OrderId:{Id}",
                model.ClientId, model.Status, model.ImplementerId, model.DateFrom, model.DateTo, model.Id);
            var element = _orderStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public List<OrderViewModel>? ReadList(OrderSearchModel? model)
        {
            _logger.LogInformation("ReadList. ClientId:{ClientId}.Status:{Status}.ImplementerId:{ImplementerId}.DateFrom:{DateFrom}.DateTo:{DateTo}OrderId:{Id}",
            model?.ClientId, model?.Status, model?.ImplementerId, model?.DateFrom, model?.DateTo, model?.Id);
            var list = model == null ? _orderStorage.GetFullList() : _orderStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public bool CreateOrder(OrderBindingModel model)
        {
            CheckModel(model);
            if (model.Status != OrderStatus.Неизвестен)
                return false;
            model.Status = OrderStatus.Принят;
            var element = _orderStorage.Insert(model);
            if (element == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            Task.Run(() => _mailWorker.MailSendAsync(new MailSendInfoBindingModel
            {
                MailAddress = element.ClientEmail,
                Subject = $"Изменение статуса заказа номер {element.Id}",
                Text = $"Ваш заказ номер {element.Id} на пиццу {element.PizzaName} от {element.DateCreate} на сумму {element.Sum} принят."
            }));
            return true;
        }

        public bool TakeOrderInWork(OrderBindingModel model)
        {
            lock (_locker)
            {
                return ChangeStatus(model, OrderStatus.Выполняется);
            }
        }

        public bool FinishOrder(OrderBindingModel model)
        {
            return ChangeStatus(model, OrderStatus.Готов);
        }

        public bool DeliveryOrder(OrderBindingModel model)
        {
            lock (_locker)
            {
                (model, var element) = FillOrderBindingModel(model);
                if (model.Status != OrderStatus.Готов && model.Status != OrderStatus.Ожидает)
                {
                    _logger.LogWarning("Changing status operation faled: Current-{Status}:required-Выдан.", model.Status);
                    throw new InvalidOperationException($"Невозможно приствоить статус выдан заказу с текущим статусом {model.Status}");
                }
                if (!_shopStorage.RestockingShops(new SupplyBindingModel
                {
                    PizzaId = model.PizzaId,
                    Count = model.Count
                }))
                {
                    if (model.Status == OrderStatus.Готов)
                    {
                        model.Status = OrderStatus.Ожидает;

                        UpdateOrder(model, element);
                    }
                    throw new ArgumentException("Недостаточно места в магазинах для поставки");
                }
                model.Status = OrderStatus.Выдан;
                return UpdateOrder(model, element);
            }
        }

        private void CheckModel(OrderBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (model.Count <= 0)
            {
                throw new ArgumentException("Колличество пиццы в заказе не может быть меньше 1", nameof(model.Count));
            }
            if (model.Sum <= 0)
            {
                throw new ArgumentException("Стоимость заказа на может быть меньше 1", nameof(model.Sum));
            }
            if (model.DateImplement.HasValue && model.DateImplement < model.DateCreate)
            {
                throw new ArithmeticException($"Дата выдачи заказа {model.DateImplement} не может быть раньше даты его создания {model.DateCreate}");
            }
            _logger.LogInformation("Pizza. PizzaId:{PizzaId}.Count:{Count}.Sum:{Sum}Id:{Id}",
                model.PizzaId, model.Count, model.Sum, model.Id);
        }
        private bool ChangeStatus(OrderBindingModel model, OrderStatus requiredStatus)
        {
            (model, var element) = FillOrderBindingModel(model);

            if (requiredStatus - model.Status == 1)
            {
                model.Status = requiredStatus;
                if (model.Status == OrderStatus.Готов)
                    model.DateImplement = DateTime.Now;
                return UpdateOrder(model, element);
            }
            _logger.LogWarning("Changing status operation faled: Current-{Status}:required-{requiredStatus}.", model.Status, requiredStatus);
            throw new InvalidOperationException($"Невозможно приствоить статус {requiredStatus} заказу с текущим статусом {model.Status}");
        }

        private (OrderBindingModel, OrderViewModel) FillOrderBindingModel(OrderBindingModel model)
        {
            CheckModel(model, false);
            var element = _orderStorage.GetElement(new OrderSearchModel()
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new InvalidOperationException(nameof(element));
            }
            model.Id = element.Id;
            model.DateCreate = element.DateCreate;
            model.PizzaId = element.PizzaId;
            model.DateImplement = element.DateImplement;
            model.ClientId = element.ClientId;
            model.Status = element.Status;
            model.Count = element.Count;
            model.Sum = element.Sum;
            if (!model.ImplementerId.HasValue)
            {
                model.ImplementerId = element.ImplementerId;
            }
            return (model, element);
        }

        private bool UpdateOrder(OrderBindingModel model, OrderViewModel MailNotificationModel)
        {
            if (_orderStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            _logger.LogWarning("Update operation sucsess");
            string DateInfo = model.DateImplement.HasValue ? $"Дата выполнения {model.DateImplement}" : "";
            Task.Run(() => _mailWorker.MailSendAsync(new MailSendInfoBindingModel
            {
                MailAddress = MailNotificationModel.ClientEmail,
                Subject = $"Изменение статуса заказа номер {MailNotificationModel.Id}",
                Text = $"Ваш заказ номер {MailNotificationModel.Id} на изделие {MailNotificationModel.PizzaName} от" +
                $" {MailNotificationModel.DateCreate} на сумму {MailNotificationModel.Sum} {model.Status}. {DateInfo}"
            }));
            return true;
        }
    }
}
