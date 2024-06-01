using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.SearchModels
{
    public class MessageInfoSearchModel
    {
        public int? ClientId { get; set; }
        public string? MessageId { get; set; }
        public int? PageLength { get; set; }
        public int? PageIndex { get; set; }
    }
}
