using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaBusinessLogic.OfficePackage
{
    public interface IDocument
    {
        public string FileName { get; set; }
        public string Title { get; set; }
    }
}
