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
    public interface IComponentLogic
    {
        List<ComponentViewModel>? ReadList(ComponentSearchModel? model);
        ComponentViewModel? ReadElement(ComponentSearchModel model);
        bool Create(ComponentBindingModel model);
        bool Update(ComponentBindingModel model);
        bool Delete(ComponentBindingModel model);
    }
}
