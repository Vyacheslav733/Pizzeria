using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.BindingModels
{
    public class ShopBindingModel : IShopModel
    {
        public int Id { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;
        public DateTime OpeningDate { get; set; } = DateTime.Now;
        public Dictionary<int, (IPizzaModel, int)> ShopPizzas { get; set; } = new();
        public int PizzaMaxCount { get; set; }
    }
}