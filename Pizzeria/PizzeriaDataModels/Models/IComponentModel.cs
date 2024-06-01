using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaDataModels.Models
{
    public interface IComponentModel : IId
    {
        string ComponentName { get; }
        double Cost { get; }
    }
}