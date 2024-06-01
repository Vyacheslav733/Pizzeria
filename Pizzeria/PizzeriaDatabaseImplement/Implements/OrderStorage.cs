using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaDatabaseImplement.Implements
{
    public class OrderStorage : IOrderStorage
    {
        public List<OrderViewModel> GetFullList()
        {
            using var context = new PizzeriaDatabase();
            return context.Orders.Include(x => x.Pizza).Include(x => x.Client).Include(y => y.Implementer).Select(x => x.GetViewModel).ToList();
        }

        public List<OrderViewModel> GetFilteredList(OrderSearchModel model)
        {
            using var context = new PizzeriaDatabase();
            if ((!model.DateFrom.HasValue || !model.DateTo.HasValue) && !model.ClientId.HasValue && !model.Status.HasValue)
            {
                return new();
            }
            return context.Orders.Include(x => x.Pizza).Include(x => x.Client).Include(x => x.Implementer).Where(x =>
                 (model.DateFrom.HasValue && model.DateTo.HasValue && x.DateCreate >= model.DateFrom && x.DateCreate <= model.DateTo) ||
                 (model.ClientId.HasValue && x.ClientId == model.ClientId) ||
                 (model.Status.HasValue && x.Status == model.Status))
                 .Select(x => x.GetViewModel).ToList();
        }

        public OrderViewModel? GetElement(OrderSearchModel model)
        {
            if (!model.Id.HasValue && (!model.ImplementerId.HasValue || !model.Status.HasValue))
            {
                return new();
            }
            using var context = new PizzeriaDatabase();
            return context.Orders.Include(x => x.Pizza).Include(x => x.Client).Include(x => x.Implementer).FirstOrDefault(x =>
                (model.Id.HasValue && x.Id == model.Id) ||
                (model.ImplementerId.HasValue && x.ImplementerId == model.ImplementerId && x.Status == model.Status))
                ?.GetViewModel;
        }

        public OrderViewModel? Insert(OrderBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            if (model == null)
                return null;
            var newOrder = Order.Create(context, model);
            if (newOrder == null)
            {
                return null;
            }
            context.Orders.Add(newOrder);
            context.SaveChanges();
            return newOrder.GetViewModel;
        }

        public OrderViewModel? Update(OrderBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            var order = context.Orders.FirstOrDefault(x => x.Id == model.Id);
            if (order == null)
            {
                return null;
            }
            order.Update(context, model);
            context.SaveChanges();
            return order.GetViewModel;
        }

        public OrderViewModel? Delete(OrderBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            var order = context.Orders.FirstOrDefault(rec => rec.Id == model.Id);
            if (order != null)
            {
                context.Orders.Remove(order);
                context.SaveChanges();
                return order.GetViewModel;
            }
            return null;
        }
    }
}
