using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.BindingModels
{
    public class MailReplySendInfoBindingModel : MailSendInfoBindingModel
    {
        public string ParentMessageId { get; set; } = string.Empty;
    }
}
