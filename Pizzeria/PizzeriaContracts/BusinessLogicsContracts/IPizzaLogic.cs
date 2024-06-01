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
    public interface IPizzaLogic
    {
        List<PizzaViewModel>? ReadList(PizzaSearchModel? model);
        PizzaViewModel? ReadElement(PizzaSearchModel model);
        bool Create(PizzaBindingModel model);
        bool Update(PizzaBindingModel model);
        bool Delete(PizzaBindingModel model);
    }
}
