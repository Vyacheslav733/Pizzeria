using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaFileImplement.Models;

namespace PizzeriaFileImplement.Implements
{
    public class PizzaStorage : IPizzaStorage
    {
        private readonly DataFileSingleton source;

        public PizzaStorage()
        {
            source = DataFileSingleton.GetInstance();
        }

        public List<PizzaViewModel> GetFullList()
        {
            return source.Pizzas.Select(x => x.GetViewModel).ToList();
        }

        public List<PizzaViewModel> GetFilteredList(PizzaSearchModel model)
        {
            if (string.IsNullOrEmpty(model.PizzaName))
            {
                return new();
            }
            return source.Pizzas.Where(x => x.PizzaName.Contains(model.PizzaName)).Select(x => x.GetViewModel).ToList();
        }

        public PizzaViewModel? GetElement(PizzaSearchModel model)
        {
            if (string.IsNullOrEmpty(model.PizzaName) && !model.Id.HasValue)
            {
                return null;
            }
            return source.Pizzas.FirstOrDefault(x =>
            (!string.IsNullOrEmpty(model.PizzaName) && x.PizzaName == model.PizzaName) ||
            (model.Id.HasValue && x.Id == model.Id))
                ?.GetViewModel;
        }

        public PizzaViewModel? Insert(PizzaBindingModel model)
        {
            model.Id = source.Pizzas.Count > 0 ? source.Pizzas.Max(x => x.Id) + 1 : 1;
            var newPizza = Pizza.Create(model);
            if (newPizza == null)
            {
                return null;
            }
            source.Pizzas.Add(newPizza);
            source.SavePizzas();
            return newPizza.GetViewModel;
        }

        public PizzaViewModel? Update(PizzaBindingModel model)
        {
            var Pizza = source.Pizzas.FirstOrDefault(x => x.Id == model.Id);
            if (Pizza == null)
            {
                return null;
            }
            Pizza.Update(model);
            source.SavePizzas();
            return Pizza.GetViewModel;
        }

        public PizzaViewModel? Delete(PizzaBindingModel model)
        {
            var Pizza = source.Pizzas.FirstOrDefault(x => x.Id == model.Id);
            if (Pizza != null)
            {
                source.Pizzas.Remove(Pizza);
                source.SavePizzas();
                return Pizza.GetViewModel;
            }
            return null;
        }
    }
}