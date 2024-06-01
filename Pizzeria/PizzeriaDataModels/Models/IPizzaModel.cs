using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaDataModels.Models
{
    public interface IPizzaModel : IId
    {
        string PizzaName { get; }
        double Price { get; }
        Dictionary<int, (IComponentModel, int)> PizzaComponents { get; }
    }
}