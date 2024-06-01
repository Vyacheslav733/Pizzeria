using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaBusinessLogic.OfficePackage.HelperModels
{
    public class WordRowParameters
    {
        public List<string> Texts { get; set; } = new();
        public WordTextProperties TextProperties { get; set; } = new();
    }
}
