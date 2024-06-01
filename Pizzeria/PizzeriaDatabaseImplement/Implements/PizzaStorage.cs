using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaDatabaseImplement.Implements
{
    public class PizzaStorage : IPizzaStorage
    {
        public List<PizzaViewModel> GetFullList()
        {
            using var context = new PizzeriaDatabase();
            return context.Pizzas.Include(x => x.Components).ThenInclude(x => x.Component).ToList()
                    .Select(x => x.GetViewModel).ToList();
        }

        public List<PizzaViewModel> GetFilteredList(PizzaSearchModel model)
        {
            if (string.IsNullOrEmpty(model.PizzaName))
            {
                return new();
            }
            using var context = new PizzeriaDatabase();
            return context.Pizzas.Include(x => x.Components).ThenInclude(x => x.Component)
                    .Where(x => x.PizzaName.Contains(model.PizzaName)).ToList().Select(x => x.GetViewModel).ToList();
        }

        public PizzaViewModel? GetElement(PizzaSearchModel model)
        {
            if (string.IsNullOrEmpty(model.PizzaName) && !model.Id.HasValue)
            {
                return null;
            }
            using var context = new PizzeriaDatabase();
            return context.Pizzas.Include(x => x.Components).ThenInclude(x => x.Component)
                .FirstOrDefault(x =>
                (!string.IsNullOrEmpty(model.PizzaName) && x.PizzaName == model.PizzaName) ||
                (model.Id.HasValue && x.Id == model.Id))
                    ?.GetViewModel;
        }

        public PizzaViewModel? Insert(PizzaBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            var newPizza = Pizza.Create(context, model);
            if (newPizza == null)
            {
                return null;
            }
            context.Pizzas.Add(newPizza);
            context.SaveChanges();
            return newPizza.GetViewModel;
        }

        public PizzaViewModel? Update(PizzaBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var Pizza = context.Pizzas.FirstOrDefault(rec => rec.Id == model.Id);
                if (Pizza == null)
                {
                    return null;
                }
                Pizza.Update(model);
                context.SaveChanges();
                Pizza.UpdateComponents(context, model);
                transaction.Commit();
                return Pizza.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public PizzaViewModel? Delete(PizzaBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            var element = context.Pizzas.Include(x => x.Components).FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Pizzas.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }
    }
}
