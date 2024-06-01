using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaListImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        private readonly DataListSingleton _source;

        public OrderStorage()
        {
            _source = DataListSingleton.GetInstance();
        }

        public List<OrderViewModel> GetFullList()
        {
            var result = new List<OrderViewModel>();
            foreach (var order in _source.Orders)
            {
                result.Add(AttachPizzaName(order.GetViewModel));
            }
            return result;
        }

        public List<OrderViewModel> GetFilteredList(OrderSearchModel model)
        {
            var result = new List<OrderViewModel>();
            if (model.DateFrom.HasValue)
            {
                foreach (var order in _source.Orders)
                {
                    if (order.DateCreate >= model.DateFrom && order.DateCreate <= model.DateTo)
                    {
                        result.Add(GetViewModel(order));
                    }
                }
            }
            else if (model.ClientId.HasValue && !model.Id.HasValue)
            {
                foreach (var order in _source.Orders)
                {
                    if (order.ClientId == model.ClientId)
                    {
                        result.Add(GetViewModel(order));
                    }
                }
            }
            else if (model.ImplementerId.HasValue && !model.Id.HasValue)
            {
                foreach (var order in _source.Orders)
                {
                    if (order.ImplementerId == model.ImplementerId)
                    {
                        result.Add(GetViewModel(order));
                    }
                }
            }
            else if (model.Id.HasValue)
            {
                foreach (var order in _source.Orders)
                {
                    if (order.Id == model.Id)
                    {
                        result.Add(GetViewModel(order));
                    }
                }
            }
            return result;
        }

        private OrderViewModel GetViewModel(Order order)
        {
            var viewModel = order.GetViewModel;
            foreach (var package in _source.Pizzas)
            {
                if (package.Id == order.PizzaId)
                {
                    viewModel.PizzaName = package.PizzaName;
                    break;
                }
            }
            foreach (var client in _source.Clients)
            {
                if (client.Id == order.ClientId)
                {
                    viewModel.ClientFIO = client.ClientFIO;
                    break;
                }
            }
            return viewModel;
        }

        public OrderViewModel? GetElement(OrderSearchModel model)
        {
            if (!model.Id.HasValue)
            {
                return null;
            }
            foreach (var order in _source.Orders)
            {
                if (model.Id.HasValue && order.Id == model.Id)
                {
                    return AttachPizzaName(order.GetViewModel);
                }
                else if (model.ImplementerId.HasValue && model.ImplementerId == order.ImplementerId)
                {
                    return GetViewModel(order);
                }
            }
            return null;
        }

        public OrderViewModel? Insert(OrderBindingModel model)
        {
            model.Id = 1;
            foreach (var order in _source.Orders)
            {
                if (model.Id <= order.Id)
                {
                    model.Id = order.Id + 1;
                }
            }
            var newOrder = Order.Create(model);
            if (newOrder == null)
            {
                return null;
            }
            _source.Orders.Add(newOrder);
            return AttachPizzaName(newOrder.GetViewModel);
        }

        public OrderViewModel? Update(OrderBindingModel model)
        {
            foreach (var order in _source.Orders)
            {
                if (order.Id == model.Id)
                {
                    order.Update(model);
                    return AttachPizzaName(order.GetViewModel);
                }
            }
            return null;
        }

        public OrderViewModel? Delete(OrderBindingModel model)
        {
            for (int i = 0; i < _source.Orders.Count; ++i)
            {
                if (_source.Orders[i].Id == model.Id)
                {
                    var element = _source.Orders[i];
                    _source.Orders.RemoveAt(i);
                    return AttachPizzaName(element.GetViewModel);
                }
            }
            return null;
        }

        private OrderViewModel AttachPizzaName(OrderViewModel model)
        {
            foreach (var pizza in _source.Pizzas)
            {
                if (pizza.Id == model.PizzaId)
                {
                    model.PizzaName = pizza.PizzaName;
                    return model;
                }
            }
            return model;
        }
    }
}
