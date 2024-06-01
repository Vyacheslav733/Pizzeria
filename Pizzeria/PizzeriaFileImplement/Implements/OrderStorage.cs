using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaFileImplement.Models;

namespace PizzeriaFileImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly DataFileSingleton source;

        public OrderStorage()
        {
            source = DataFileSingleton.GetInstance();
        }

        public List<OrderViewModel> GetFullList() => source.Orders.Select(x => AttachPizzaName(x.GetViewModel)).ToList();

        public List<OrderViewModel> GetFilteredList(OrderSearchModel model)
        {
            if (model.DateFrom.HasValue)
            {
                return source.Orders.Where(x => x.DateCreate >= model.DateFrom && x.DateCreate <= model.DateTo)
                    .Select(x => GetViewModel(x)).ToList();
            }

            if (model.ClientId.HasValue && !model.Id.HasValue)
            {
                return source.Orders.Where(x => x.ClientId == model.ClientId).Select(x => x.GetViewModel).ToList();
            }

            if (!model.ImplementerId.HasValue && !model.Id.HasValue)
            {
                return source.Orders.Where(x => x.ImplementerId == model.ImplementerId).Select(x => x.GetViewModel).ToList();

            }

            if (model.Id.HasValue)
            {
                return source.Orders.Where(x => x.Id.Equals(model.Id)).Select(x => GetViewModel(x)).ToList();
            }
            return new();
        }

        public OrderViewModel? GetElement(OrderSearchModel model)
        {
            if (model.ImplementerId.HasValue)
            {
                return source.Orders.FirstOrDefault(x => x.ImplementerId == model.ImplementerId)?.GetViewModel;
            }
            if (!model.Id.HasValue)
            {
                return new();
            }
            return AttachPizzaName(source.Orders.FirstOrDefault(x => x.Id == model.Id)?.GetViewModel);
        }

        private OrderViewModel GetViewModel(Order order)
        {
            var viewModel = order.GetViewModel;

            var pizza = source.Pizzas.FirstOrDefault(x => x.Id == order.PizzaId);
            var client = source.Clients.FirstOrDefault(x => x.Id == order.ClientId);

            if (pizza != null)
            {
                viewModel.PizzaName = pizza.PizzaName;
            }
            if (client != null)
            {
                viewModel.ClientFIO = client.ClientFIO;
            }
            return viewModel;
        }

        public OrderViewModel? Insert(OrderBindingModel model)
        {
            model.Id = source.Orders.Count > 0 ? source.Orders.Max(x => x.Id) + 1 : 1;
            var newOrder = Order.Create(model);
            if (newOrder == null)
            {
                return null;
            }
            source.Orders.Add(newOrder);
            source.SaveOrders();
            return AttachPizzaName(newOrder.GetViewModel);
        }

        public OrderViewModel? Update(OrderBindingModel model)
        {
            var order = source.Orders.FirstOrDefault(x => x.Id == model.Id);
            if (order == null)
            {
                return null;
            }
            order.Update(model);
            source.SaveOrders();
            return AttachPizzaName(order.GetViewModel);
        }

        public OrderViewModel? Delete(OrderBindingModel model)
        {
            var order = source.Orders.FirstOrDefault(x => x.Id == model.Id);
            if (order != null)
            {
                source.Orders.Remove(order);
                source.SaveOrders();
                return AttachPizzaName(order.GetViewModel);
            }
            return null;
        }

        private OrderViewModel? AttachPizzaName(OrderViewModel? model)
        {
            if (model == null)
            {
                return null;
            }
            var pizza = source.Pizzas.FirstOrDefault(x => x.Id == model.PizzaId);
            if (pizza != null)
            {
                model.PizzaName = pizza.PizzaName;
            }
            return model;
        }
    }

}