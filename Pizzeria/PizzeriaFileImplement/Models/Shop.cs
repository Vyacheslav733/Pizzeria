using PizzeriaDataModels.Models;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace PizzeriaFileImplement.Models
{
    [DataContract]
    public class Shop : IShopModel
    {
        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string ShopName { get; private set; } = string.Empty;

        [DataMember]
        public string Adress { get; private set; } = string.Empty;

        [DataMember]
        public DateTime OpeningDate { get; private set; }
        public Dictionary<int, int> Pizzas { get; private set; } = new();
        private Dictionary<int, (IPizzaModel, int)>? _shopPizzas = null;

        [DataMember]
        public Dictionary<int, (IPizzaModel, int)> ShopPizzas
        {
            get
            {
                if (_shopPizzas == null)
                {
                    var source = DataFileSingleton.GetInstance();
                    _shopPizzas = Pizzas.ToDictionary(x => x.Key, y => ((source.Pizzas.FirstOrDefault(z => z.Id == y.Key) as IPizzaModel)!, y.Value));
                }
                return _shopPizzas;
            }
        }

        [DataMember]
        public int PizzaMaxCount { get; private set; }

        public static Shop? Create(ShopBindingModel? model)
        {
            if (model == null)
            {
                return null;
            }
            return new Shop()
            {
                Id = model.Id,
                ShopName = model.ShopName,
                Adress = model.Adress,
                OpeningDate = model.OpeningDate,
                Pizzas = model.ShopPizzas.ToDictionary(x => x.Key, x => x.Value.Item2),
                PizzaMaxCount = model.PizzaMaxCount
            };
        }

        public static Shop? Create(XElement element)
        {
            if (element == null)
            {
                return null;
            }
            return new()
            {
                Id = Convert.ToInt32(element.Attribute("Id")!.Value),
                ShopName = element.Element("ShopName")!.Value,
                Adress = element.Element("Adress")!.Value,
                OpeningDate = Convert.ToDateTime(element.Element("OpeningDate")!.Value),
                Pizzas = element.Element("ShopPizzas")!.Elements("ShopPizza")!.ToDictionary(x => Convert.ToInt32(x.Element("Key")?.Value),
                x => Convert.ToInt32(x.Element("Value")?.Value)),
                PizzaMaxCount = Convert.ToInt32(element.Element("PizzaMaxCount")!.Value)
            };
        }

        public void Update(ShopBindingModel? model)
        {
            if (model == null)
            {
                return;
            }
            ShopName = model.ShopName;
            Adress = model.Adress;
            OpeningDate = model.OpeningDate;
            PizzaMaxCount = model.PizzaMaxCount;
            Pizzas = model.ShopPizzas.ToDictionary(x => x.Key, x => x.Value.Item2);
            _shopPizzas = null;
        }

        public ShopViewModel GetViewModel => new()
        {
            Id = Id,
            ShopName = ShopName,
            Adress = Adress,
            OpeningDate = OpeningDate,
            ShopPizzas = ShopPizzas,
            PizzaMaxCount = PizzaMaxCount
        };

        public XElement GetXElement => new("Shop",
            new XAttribute("Id", Id),
            new XElement("ShopName", ShopName),
            new XElement("Adress", Adress),
            new XElement("OpeningDate", OpeningDate.ToString()),
            new XElement("ShopPizzas", Pizzas.Select(
                x => new XElement("ShopPizza", new XElement("Key", x.Key), new XElement("Value", x.Value))).ToArray()),
            new XElement("PizzaMaxCount", PizzaMaxCount.ToString())
            );

        public void PizzasUpdate()
        {
            _shopPizzas = null;
        }
    }
}