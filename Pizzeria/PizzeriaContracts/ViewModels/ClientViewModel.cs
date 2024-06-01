using PizzeriaContracts.Attributes;
using PizzeriaDataModels.Models;
using System.ComponentModel;

namespace PizzeriaContracts.ViewModels
{
    public class ClientViewModel : IClientModel
    {
        [Column(visible: false)]
        public int Id { get; set; }
        [Column(title: "ФИО клиента", width: 150)]
        public string ClientFIO { get; set; } = string.Empty;
        [Column(title: "Логин (эл. почта)", gridViewAutoSize: GridViewAutoSize.Fill, isUseAutoSize: true)]
        public string Email { get; set; } = string.Empty;
        [Column(title: "Пароль", width: 150)]
        public string Password { get; set; } = string.Empty;
    }
}
