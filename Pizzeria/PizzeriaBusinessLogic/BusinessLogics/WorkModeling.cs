using Microsoft.Extensions.Logging;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Enums;

namespace PizzeriaBusinessLogic.BusinessLogics
{
    public class WorkModeling : IWorkProcess
    {
        private readonly ILogger _logger;

        private readonly Random _rnd;

        private IOrderLogic? _orderLogic;

        public WorkModeling(ILogger<WorkModeling> logger)
        {
            _logger = logger;
            _rnd = new Random(1000);
        }

        public void DoWork(IImplementerLogic implementerLogic, IOrderLogic orderLogic)
        {
            _orderLogic = orderLogic;
            var implementers = implementerLogic.ReadList(null);
            if (implementers == null)
            {
                _logger.LogWarning("DoWork. Implementers is null");
                return;
            }
            var orders = _orderLogic.ReadList(new OrderSearchModel { Status = OrderStatus.Выдан });
            int count = _orderLogic.ReadList(null).Count;
            if (orders == null || count == orders.Count)
            {
                _logger.LogWarning("DoWork. Orders is null or empty");
                return;
            }
            orders = _orderLogic.ReadList(null);
            _logger.LogDebug("DoWork for {Count} orders", orders.Count);
            foreach (var implementer in implementers)
            {
                Task.Run(() => WorkerWorkAsync(implementer, orders));
            }
        }

        private async Task WorkerWorkAsync(ImplementerViewModel implementer, List<OrderViewModel> orders)
        {
            if (_orderLogic == null || implementer == null)
            {
                return;
            }
            await DeliverWaitingOrder(implementer);
            await RunOrderInWork(implementer);

            await Task.Run(() =>
            {
                foreach (var order in orders)
                {
                    try
                    {
                        _logger.LogDebug("DoWork. Worker {Id} try get order {Order}", implementer.Id, order.Id);
                        // пытаемся назначить заказ на исполнителя
                        _orderLogic.TakeOrderInWork(new OrderBindingModel
                        {
                            Id = order.Id,
                            ImplementerId = implementer.Id
                        });
                        // делаем работу
                        Thread.Sleep(implementer.WorkExperience * _rnd.Next(100, 1000) * order.Count);
                        _logger.LogDebug("DoWork. Worker {Id} finish order {Order}", implementer.Id, order.Id);
                        _orderLogic.FinishOrder(new OrderBindingModel
                        {
                            Id = order.Id
                        });
                        _orderLogic.DeliveryOrder(new OrderBindingModel { Id = order.Id });
                    }
                    // кто-то мог уже перехватить заказ, игнорируем ошибку
                    catch (InvalidOperationException ex)
                    {
                        _logger.LogWarning(ex, "Error try get work");
                    }
                    // заканчиваем выполнение имитации в случае иной ошибки
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error while do work");
                        throw;
                    }
                    // отдыхаем
                    Thread.Sleep(implementer.Qualification * _rnd.Next(10, 100));
                }
            });
        }

        private async Task RunOrderInWork(ImplementerViewModel implementer)
        {
            if (_orderLogic == null || implementer == null)
            {
                return;
            }
            try
            {
                var runOrder = await Task.Run(() => _orderLogic.ReadElement(new OrderSearchModel
                {
                    ImplementerId = implementer.Id,
                    Status = OrderStatus.Выполняется
                }));
                if (runOrder == null)
                {
                    return;
                }

                _logger.LogDebug("DoWork. Worker {Id} back to order {Order}", implementer.Id, runOrder.Id);
                // доделываем работу
                Thread.Sleep(implementer.WorkExperience * _rnd.Next(100, 300) * runOrder.Count);
                _logger.LogDebug("DoWork. Worker {Id} finish order {Order}", implementer.Id, runOrder.Id);
                _orderLogic.FinishOrder(new OrderBindingModel
                {
                    Id = runOrder.Id
                });
                _orderLogic.DeliveryOrder(new OrderBindingModel { Id = runOrder.Id });
                // отдыхаем
                Thread.Sleep(implementer.Qualification * _rnd.Next(10, 100));
            }
            // заказа может не быть, просто игнорируем ошибку
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error try get work");
            }
            // а может возникнуть иная ошибка, тогда просто заканчиваем выполнение имитации
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while do work");
                throw;
            }
        }

        private async Task DeliverWaitingOrder(ImplementerViewModel implementer)
        {
            if (_orderLogic == null || implementer == null)
            {
                return;
            }
            var waitingOrders = await Task.Run(() => _orderLogic.ReadList(new OrderSearchModel
            {
                ImplementerId = implementer.Id,
                Status = OrderStatus.Ожидает
            }));
            if (waitingOrders == null || waitingOrders.Count == 0)
            {
                return;
            }
            _logger.LogInformation("DeliverWaitingOrder. Find some waitig order for implementer:{id}.Count:{count}", implementer.Id, waitingOrders.Count);
            foreach (var waitingOrder in waitingOrders)
            {
                try
                {
                    _logger.LogInformation("DeliverWaitingOrder. Trying to deliver order id:{id}", waitingOrder.Id);
                    var res = _orderLogic.DeliveryOrder(new OrderBindingModel
                    {
                        Id = waitingOrder.Id
                    });
                }
                catch (ArgumentException ex)
                {
                    _logger.LogWarning(ex, "DeliverWaitingOrder. Fault");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning(ex, "Error try deliver order");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while do work");
                    throw;
                }
            }
        }
    }
}
