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
    public class PizzaViewModel : IPizzaModel
    {
        [Column(visible: false)]
        public int Id { get; set; }

        [Column(title: "Название пиццы", gridViewAutoSize: GridViewAutoSize.Fill, isUseAutoSize: true)]
        public string PizzaName { get; set; } = string.Empty;

        [Column(title: "Цена", width: 70)]
        public double Price { get; set; }

        [Column(visible: false)]
        public Dictionary<int, (IComponentModel, int)> PizzaComponents { get; set; } = new();
    }
}