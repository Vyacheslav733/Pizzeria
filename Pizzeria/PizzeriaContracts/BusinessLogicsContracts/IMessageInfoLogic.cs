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
    public interface IMessageInfoLogic
    {
        List<MessageInfoViewModel>? ReadList(MessageInfoSearchModel? model);
        MessageInfoViewModel? ReadElement(MessageInfoSearchModel model);
        bool Create(MessageInfoBindingModel model);
        bool Update(MessageInfoBindingModel model);
    }
}
