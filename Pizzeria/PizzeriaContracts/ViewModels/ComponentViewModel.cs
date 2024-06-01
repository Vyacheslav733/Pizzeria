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
    public class ComponentViewModel : IComponentModel
    {
        [Column(visible: false)]
        public int Id { get; set; }

        [Column(title: "Название ингридиента", gridViewAutoSize: GridViewAutoSize.Fill, isUseAutoSize: true)]
        public string ComponentName { get; set; } = string.Empty;

        [Column(title: "Цена", width: 150)]
        public double Cost { get; set; }
    }
}