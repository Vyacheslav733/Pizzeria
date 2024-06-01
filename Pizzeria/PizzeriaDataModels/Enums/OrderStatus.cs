using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaDataModels.Enums
{
    public enum OrderStatus
    {
        Неизвестен = -1,
        Принят = 0,
        Выполняется = 1,
        Готов = 2,
        Ожидает = 3,
        Выдан = 4
    }
}