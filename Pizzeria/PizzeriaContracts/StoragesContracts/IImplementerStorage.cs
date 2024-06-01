using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.ViewModels;

namespace PizzeriaContracts.StoragesContracts
{
    public interface IImplementerStorage
    {
        List<ImplementerViewModel> GetFullList();
        List<ImplementerViewModel> GetFilteredList(ImplementerSearchModel model);
        ImplementerViewModel? GetElement(ImplementerSearchModel model);
        ImplementerViewModel? Insert(ImplementerBindingModel model);
        ImplementerViewModel? Update(ImplementerBindingModel model);
        ImplementerViewModel? Delete(ImplementerBindingModel model);
    }
}
