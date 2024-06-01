using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.StoragesContracts
{
    public interface IPizzaStorage
    {
        List<PizzaViewModel> GetFullList();
        List<PizzaViewModel> GetFilteredList(PizzaSearchModel model);
        PizzaViewModel? GetElement(PizzaSearchModel model);
        PizzaViewModel? Insert(PizzaBindingModel model);
        PizzaViewModel? Update(PizzaBindingModel model);
        PizzaViewModel? Delete(PizzaBindingModel model);
    }
}
