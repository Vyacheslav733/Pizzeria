using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Models;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace PizzeriaFileImplement.Models
{
    [DataContract]
    public class Pizza : IPizzaModel
    {
        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string PizzaName { get; private set; } = string.Empty;

        [DataMember]
        public double Price { get; private set; }
        public Dictionary<int, int> Components { get; private set; } = new();
        private Dictionary<int, (IComponentModel, int)>? _pizzaComponents = null;

        [DataMember]
        public Dictionary<int, (IComponentModel, int)> PizzaComponents
        {
            get
            {
                if (_pizzaComponents == null)
                {
                    var source = DataFileSingleton.GetInstance();
                    _pizzaComponents = Components.ToDictionary(x => x.Key, y => ((source.Components.FirstOrDefault(z => z.Id == y.Key) as IComponentModel)!, y.Value));
                }
                return _pizzaComponents;
            }
        }
        public static Pizza? Create(PizzaBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Pizza()
            {
                Id = model.Id,
                PizzaName = model.PizzaName,
                Price = model.Price,
                Components = model.PizzaComponents.ToDictionary(x => x.Key, x => x.Value.Item2)
            };
        }
        public static Pizza? Create(XElement element)
        {
            if (element == null)
            {
                return null;
            }
            return new Pizza()
            {
                Id = Convert.ToInt32(element.Attribute("Id")!.Value),
                PizzaName = element.Element("PizzaName")!.Value,
                Price = Convert.ToDouble(element.Element("Price")!.Value),
                Components = element.Element("PizzaComponents")!.Elements("PizzaComponent").ToDictionary(x => Convert.ToInt32(x.Element("Key")?.Value),
                x => Convert.ToInt32(x.Element("Value")?.Value))
            };
        }
        public void Update(PizzaBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            PizzaName = model.PizzaName;
            Price = model.Price;
            Components = model.PizzaComponents.ToDictionary(x => x.Key, x => x.Value.Item2);
            _pizzaComponents = null;
        }
        public PizzaViewModel GetViewModel => new()
        {
            Id = Id,
            PizzaName = PizzaName,
            Price = Price,
            PizzaComponents = PizzaComponents
        };
        public XElement GetXElement => new("Pizza",
                new XAttribute("Id", Id),
                new XElement("PizzaName", PizzaName),
                new XElement("Price", Price.ToString()),
                new XElement("PizzaComponents", Components.Select(
                    x => new XElement("PizzaComponent", new XElement("Key", x.Key), new XElement("Value", x.Value))).ToArray()));
    }
}