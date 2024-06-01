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
    public interface IMessageInfoStorage
    {
        List<MessageInfoViewModel> GetFullList();
        List<MessageInfoViewModel> GetFilteredList(MessageInfoSearchModel model);
        MessageInfoViewModel? GetElement(MessageInfoSearchModel model);
        MessageInfoViewModel? Insert(MessageInfoBindingModel model);
        MessageInfoViewModel? Update(MessageInfoBindingModel model);
    }
}
