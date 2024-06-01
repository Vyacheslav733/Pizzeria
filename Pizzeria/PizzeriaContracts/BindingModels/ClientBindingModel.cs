using PizzeriaDataModels.Models;

namespace PizzeriaContracts.BindingModels
{
    public class ClientBindingModel : IClientModel
    {
        public int Id { get; set; }
        public string ClientFIO { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
