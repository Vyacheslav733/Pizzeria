using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Enums;
using PizzeriaDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PizzeriaDatabaseImplement.Models
{
    [DataContract]
    public class Order : IOrderModel
    {
        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        [Required]
        public int ClientId { get; private set; }

        public virtual Client Client { get; private set; } = new();

        [DataMember]
        [Required]
        public int PizzaId { get; private set; }

        public virtual Pizza Pizza { get; set; } = new();

        [DataMember]
        public int? ImplementerId { get; private set; }

        public virtual Implementer? Implementer { get; set; } = new();

        [DataMember]
        [Required]
        public int Count { get; private set; }

        [DataMember]
        [Required]
        public double Sum { get; private set; }

        [DataMember]
        [Required]
        public OrderStatus Status { get; private set; } = OrderStatus.Неизвестен;

        [DataMember]
        [Required]
        public DateTime DateCreate { get; private set; } = DateTime.Now;

        [DataMember]
        public DateTime? DateImplement { get; private set; }

        public static Order Create(PizzeriaDatabase context, OrderBindingModel model)
        {
            return new Order()
            {
                Id = model.Id,
                ClientId = model.ClientId,
                Client = context.Clients.First(x => x.Id == model.ClientId),
                PizzaId = model.PizzaId,
                Pizza = context.Pizzas.First(x => x.Id == model.PizzaId),
                ImplementerId = model.ImplementerId,
                Implementer = model.ImplementerId.HasValue ? context.Implementers.First(x => x.Id == model.ImplementerId) : null,
                Count = model.Count,
                Sum = model.Sum,
                Status = model.Status,
                DateCreate = model.DateCreate,
                DateImplement = model.DateImplement,
            };
        }

        public void Update(PizzeriaDatabase context, OrderBindingModel? model)
        {
            if (model == null)
            {
                return;
            }
            Status = model.Status;
            DateImplement = model.DateImplement;
            ImplementerId = model.ImplementerId;
            Implementer = model.ImplementerId.HasValue ? context.Implementers.First(x => x.Id == model.ImplementerId) : null;
        }

        public OrderViewModel GetViewModel => new()
        {
            Id = Id,
            ClientId = ClientId,
            ClientFIO = Client.ClientFIO,
            ClientEmail = Client.Email,
            PizzaId = PizzaId,
            PizzaName = Pizza.PizzaName,
            ImplementerId = ImplementerId,
            ImplementerFIO = Implementer != null ? Implementer.ImplementerFIO : null,
            Count = Count,
            Sum = Sum,
            Status = Status,
            DateCreate = DateCreate,
            DateImplement = DateImplement,
        };
    }
}
