using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaDatabaseImplement.Models
{
    [DataContract]
    public class Client : IClientModel
	{
        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        [Required]
		public string ClientFIO { get; set; } = string.Empty;

        [DataMember]
        [Required]
		public string Email { get; set; } = string.Empty;

        [DataMember]
        [Required]
		public string Password { get; set; } = string.Empty;

		[ForeignKey("ClientId")]
		public virtual List<Order> ClientOrders { get; set; } = new();

		[ForeignKey("ClientId")]
		public virtual List<MessageInfo> ClientMessages { get; set; } = new();

		public static Client? Create(ClientBindingModel model)
		{
			if (model == null)
			{
				return null;
			}
			return new Client()
			{
				Id = model.Id,
				ClientFIO = model.ClientFIO,
				Email = model.Email,
				Password = model.Password
			};
		}

		public static Client Create(ClientViewModel model)
		{
			return new Client
			{
				Id = model.Id,
				ClientFIO = model.ClientFIO,
				Email = model.Email,
				Password = model.Password
			};
		}

		public void Update(ClientBindingModel model)
		{
			if (model == null)
			{
				return;
			}
			ClientFIO = model.ClientFIO;
			Email = model.Email;
			Password = model.Password;
		}

		public ClientViewModel GetViewModel => new()
		{
			Id = Id,
			ClientFIO = ClientFIO,
			Email = Email,
			Password = Password
		};
	}
}
