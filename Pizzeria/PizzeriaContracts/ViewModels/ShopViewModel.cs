using PizzeriaContracts.Attributes;
using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.ViewModels
{

    public class ShopViewModel : IShopModel
    {
        [Column(visible: false)]
        public int Id { get; set; }

        [Column(title: "Магазин", width: 200)]
        public string ShopName { get; set; } = string.Empty;

        [Column(title: "Адрес", width: 100)]
        public string Adress { get; set; } = string.Empty;

        [Column(title: "Дата открытия", width: 100, format: "d")]
        public DateTime OpeningDate { get; set; }

        [Column(visible: false)]
        public Dictionary<int, (IPizzaModel, int)> ShopPizzas { get; set; } = new();

        [Column(title: "Вместимость", width: 100)]
        public int PizzaMaxCount { get; set; }
    }
}