using PizzeriaDataModels;
using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaDataModels.Models
{
    public interface IShopModel : IId
    {
        string ShopName { get; }
        string Adress { get; }
        DateTime OpeningDate { get; }
        Dictionary<int, (IPizzaModel, int)> ShopPizzas { get; }
        public int PizzaMaxCount { get; }
    }
}