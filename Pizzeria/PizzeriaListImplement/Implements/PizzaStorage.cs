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
    public class PizzaStorage : IPizzaStorage
    {
        private readonly DataListSingleton _source;

        public PizzaStorage()
        {
            _source = DataListSingleton.GetInstance();
        }

        public List<PizzaViewModel> GetFullList()
        {
            var result = new List<PizzaViewModel>();
            foreach (var pizzas in _source.Pizzas)
            {
                result.Add(pizzas.GetViewModel);
            }
            return result;
        }

        public List<PizzaViewModel> GetFilteredList(PizzaSearchModel model)
        {
            var result = new List<PizzaViewModel>();
            if (string.IsNullOrEmpty(model.PizzaName))
            {
                return result;
            }
            foreach (var pizzas in _source.Pizzas)
            {
                if (pizzas.PizzaName.Contains(model.PizzaName))
                {
                    result.Add(pizzas.GetViewModel);
                }
            }
            return result;
        }

        public PizzaViewModel? GetElement(PizzaSearchModel model)
        {
            if (string.IsNullOrEmpty(model.PizzaName) && !model.Id.HasValue)
            {
                return null;
            }
            foreach (var pizzas in _source.Pizzas)
            {
                if ((!string.IsNullOrEmpty(model.PizzaName) && pizzas.PizzaName == model.PizzaName) ||
                    (model.Id.HasValue && pizzas.Id == model.Id))
                {
                    return pizzas.GetViewModel;
                }
            }
            return null;
        }

        public PizzaViewModel? Insert(PizzaBindingModel model)
        {
            model.Id = 1;
            foreach (var pizzas in _source.Pizzas)
            {
                if (model.Id <= pizzas.Id)
                {
                    model.Id = pizzas.Id + 1;
                }
            }
            var newPizzas = Pizza.Create(model);
            if (newPizzas == null)
            {
                return null;
            }
            _source.Pizzas.Add(newPizzas);
            return newPizzas.GetViewModel;
        }

        public PizzaViewModel? Update(PizzaBindingModel model)
        {
            foreach (var pizzas in _source.Pizzas)
            {
                if (pizzas.Id == model.Id)
                {
                    pizzas.Update(model);
                    return pizzas.GetViewModel;
                }
            }
            return null;
        }

        public PizzaViewModel? Delete(PizzaBindingModel model)
        {
            for (int i = 0; i < _source.Pizzas.Count; ++i)
            {
                if (_source.Pizzas[i].Id == model.Id)
                {
                    var element = _source.Pizzas[i];
                    _source.Pizzas.RemoveAt(i);
                    return element.GetViewModel;
                }
            }
            return null;
        }
    }
}
