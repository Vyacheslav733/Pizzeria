using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.ViewModels
{
    public class ReportShopsViewModel
    {
        public string ShopName { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public List<(string Pizza, int count)> Pizzas { get; set; } = new();
    }
}
