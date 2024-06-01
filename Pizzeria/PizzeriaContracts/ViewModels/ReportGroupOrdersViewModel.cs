using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.ViewModels
{
    public class ReportGroupOrdersViewModel
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public int OrdersCount { get; set; }
        public double OrdersSum { get; set; }
    }
}
