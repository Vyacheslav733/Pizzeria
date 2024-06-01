using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaListImplement.Models
{
    public class Pizza : IPizzaModel
    {
        public int Id { get; private set; }
        public string PizzaName { get; private set; } = string.Empty;
        public double Price { get; private set; }
        public Dictionary<int, (IComponentModel, int)> PizzaComponents { get; private set; } = new Dictionary<int, (IComponentModel, int)>();

        public static Pizza? Create(PizzaBindingModel? model)
        {
            if (model == null)
            {
                return null;
            }
            return new Pizza()
            {
                Id = model.Id,
                PizzaName = model.PizzaName,
                Price = model.Price,
                PizzaComponents = model.PizzaComponents,
            };
        }

        public void Update(PizzaBindingModel? model)
        {
            if (model == null)
            {
                return;
            }
            PizzaName = model.PizzaName;
            Price = model.Price;
            PizzaComponents = model.PizzaComponents;
        }

        public PizzaViewModel GetViewModel => new()
        {
            Id = Id,
            PizzaName = PizzaName,
            Price = Price,
            PizzaComponents = PizzaComponents
        };
    }
}
