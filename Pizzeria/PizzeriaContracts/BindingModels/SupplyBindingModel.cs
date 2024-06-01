using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.BindingModels
{
    public class SupplyBindingModel : ISupplyModel
    {
        public int ShopId { get; set; }
        public int PizzaId { get; set; }
        public int Count { get; set; }
    }
}