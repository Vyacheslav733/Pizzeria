using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.BindingModels
{
    public class PizzaBindingModel : IPizzaModel
    {
        public int Id { get; set; }
        public string PizzaName { get; set; } = string.Empty;
        public double Price { get; set; }
        public Dictionary<int, (IComponentModel, int)> PizzaComponents { get; set; } = new();
    }
}