using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.BusinessLogicsContracts
{
    public interface IOrderLogic
    {
        List<OrderViewModel>? ReadList(OrderSearchModel? model);
        bool CreateOrder(OrderBindingModel model);
        bool TakeOrderInWork(OrderBindingModel model);
        bool FinishOrder(OrderBindingModel model);
        bool DeliveryOrder(OrderBindingModel model);
        OrderViewModel? ReadElement(OrderSearchModel model);
    }
}
