using PizzeriaContracts.Attributes;
using PizzeriaDataModels.Enums;
using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaContracts.ViewModels
{
    public class OrderViewModel : IOrderModel
    {
        [Column(title: "Номер", width: 90)]
        public int Id { get; set; }

        [Column(visible: false)]
        public int ClientId { get; set; }

        [Column(title: "Имя клиента", width: 190)]
        public string ClientFIO { get; set; } = string.Empty;

        [Column(visible: false)]
        public string ClientEmail { get; set; } = string.Empty;

        [Column(visible: false)]
        public int? ImplementerId { get; set; }

        [Column(title: "Исполнитель", width: 150)]
        public string? ImplementerFIO { get; set; } = null;

        [Column(visible: false)]
        public int PizzaId { get; set; }

        [Column(title: "Пицца", gridViewAutoSize: GridViewAutoSize.Fill, isUseAutoSize: true)]
        public string PizzaName { get; set; } = string.Empty;

        [Column(title: "Количество", width: 100)]
        public int Count { get; set; }

        [Column(title: "Сумма", width: 120)]
        public double Sum { get; set; }

        [Column(title: "Статус", width: 70)]
        public OrderStatus Status { get; set; } = OrderStatus.Неизвестен;

        [Column(title: "Дата создания", width: 120)]
        public DateTime DateCreate { get; set; } = DateTime.Now;

        [Column(title: "Дата выполнения", width: 120)]
        public DateTime? DateImplement { get; set; }
    }
}