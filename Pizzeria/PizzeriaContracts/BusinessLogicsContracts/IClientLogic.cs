using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaContracts.SearchModels;

namespace PizzeriaContracts.BusinessLogicsContracts
{
    public interface IClientLogic
    {
        List<ClientViewModel>? ReadList(ClientSearchModel? model);
        ClientViewModel? ReadElement(ClientSearchModel model);
        bool Create(ClientBindingModel model);
        bool Update(ClientBindingModel model);
        bool Delete(ClientBindingModel model);
    }
}
