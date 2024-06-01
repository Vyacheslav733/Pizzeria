using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.ViewModels;

namespace PizzeriaContracts.BusinessLogicsContracts
{
    public interface IImplementerLogic
    {
        List<ImplementerViewModel>? ReadList(ImplementerSearchModel? model);
        ImplementerViewModel? ReadElement(ImplementerSearchModel model);
        bool Create(ImplementerBindingModel model);
        bool Update(ImplementerBindingModel model);
        bool Delete(ImplementerBindingModel model);
    }
}
