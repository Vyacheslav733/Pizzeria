using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaDataModels.Models
{
    public interface ISupplyModel
    {
        int ShopId { get; }
        int PizzaId { get; }
        int Count { get; }
    }
}