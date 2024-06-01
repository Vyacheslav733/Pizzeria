using PizzeriaDataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaDataModels.Models
{
    public interface IOrderModel : IId
    {
        int PizzaId { get; }
        int ClientId { get; }
        int? ImplementerId { get; }
        int Count { get; }
        double Sum { get; }
        OrderStatus Status { get; }
        DateTime DateCreate { get; }
        DateTime? DateImplement { get; }
    }
}