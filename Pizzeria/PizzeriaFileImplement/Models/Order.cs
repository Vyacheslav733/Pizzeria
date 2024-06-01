using PizzeriaDataModels.Enums;
using PizzeriaDataModels.Models;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace PizzeriaFileImplement.Models
{
    [DataContract]
    public class Order : IOrderModel
    {
        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public int ClientId { get; private set; }

        [DataMember]
        public int? ImplementerId { get; set; }

        [DataMember]
        public int PizzaId { get; private set; }

        [DataMember]
        public int Count { get; private set; }

        [DataMember]
        public double Sum { get; private set; }

        [DataMember]
        public OrderStatus Status { get; private set; } = OrderStatus.Неизвестен;

        [DataMember]
        public DateTime DateCreate { get; private set; } = DateTime.Now;

        [DataMember]
        public DateTime? DateImplement { get; private set; }

        public static Order? Create(OrderBindingModel? model)
        {
            if (model == null)
            {
                return null;
            }
            return new Order()
            {
                Id = model.Id,
                PizzaId = model.PizzaId,
                ClientId = model.ClientId,
                ImplementerId = model.ImplementerId,
                Count = model.Count,
                Sum = model.Sum,
                Status = model.Status,
                DateCreate = model.DateCreate,
                DateImplement = model.DateImplement,
            };
        }

        public static Order? Create(XElement element)
        {
            if (element == null)
            {
                return null;
            }
            string dateImplement = element.Element("DateImplement")!.Value;
            return new Order()
            {
                Id = Convert.ToInt32(element.Attribute("Id")!.Value),
                PizzaId = Convert.ToInt32(element.Element("PizzaId")!.Value),
                ClientId = Convert.ToInt32(element.Element("ClientId")!.Value),
                ImplementerId = Convert.ToInt32(element.Element("ImplementerId")!.Value),
                Count = Convert.ToInt32(element.Element("Count")!.Value),
                Sum = Convert.ToDouble(element.Element("Sum")!.Value),
                Status = (OrderStatus)(Enum.Parse(typeof(OrderStatus), element.Element("Status")!.Value)),
                DateCreate = Convert.ToDateTime(element.Element("DateCreate")!.Value),
                DateImplement = (dateImplement == "" || dateImplement is null) ? Convert.ToDateTime(null) : Convert.ToDateTime(dateImplement)
            };

        }

        public void Update(OrderBindingModel? model)
        {
            if (model == null)
            {
                return;
            }
            Status = model.Status;
            if (model.Status == OrderStatus.Выдан) DateImplement = model.DateImplement;
        }

        public OrderViewModel GetViewModel => new()
        {
            Id = Id,
            PizzaId = PizzaId,
            ClientId = ClientId,
            ImplementerId = ImplementerId,
            Count = Count,
            Sum = Sum,
            Status = Status,
            DateCreate = DateCreate,
            DateImplement = DateImplement,
        };

        public XElement GetXElement => new("Order",
           new XAttribute("Id", Id),
           new XElement("PizzaId", PizzaId.ToString()),
           new XElement("ClientId", ClientId.ToString()),
           new XElement("ImplementerId", ImplementerId),
           new XElement("Count", Count.ToString()),
           new XElement("Sum", Sum.ToString()),
           new XElement("Status", Status.ToString()),
           new XElement("DateCreate", DateCreate.ToString()),
           new XElement("DateImplement", DateImplement.ToString()));
    }
}