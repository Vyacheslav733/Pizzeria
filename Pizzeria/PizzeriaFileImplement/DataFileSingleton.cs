using PizzeriaFileImplement.Models;
using System.Xml.Linq;

namespace PizzeriaFileImplement
{
    public class DataFileSingleton
    {
        private static DataFileSingleton? instance;
        private readonly string ComponentFileName = "Component.xml";
        private readonly string OrderFileName = "Order.xml";
        private readonly string PizzaFileName = "Pizza.xml";
        private readonly string ClientFileName = "Client.xml";
        private readonly string ImplementerFileName = "Implementer.xml";
        private readonly string ShopFileName = "Shop.xml";
        private readonly string MessageInfoFileName = "MessageInfo.xml";
        public List<Component> Components { get; private set; }
        public List<Order> Orders { get; private set; }
        public List<Pizza> Pizzas { get; private set; }
        public List<Client> Clients { get; private set; }
        public List<Implementer> Implementers { get; private set; }
        public List<Shop> Shops { get; private set; }
        public List<MessageInfo> Messages { get; private set; }


        public static DataFileSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataFileSingleton();
            }
            return instance;
        }

        public void SaveComponents() => SaveData(Components, ComponentFileName, "Components", x => x.GetXElement);
        public void SavePizzas() => SaveData(Pizzas, PizzaFileName, "Pizzas", x => x.GetXElement);
        public void SaveOrders() => SaveData(Orders, OrderFileName, "Orders", x => x.GetXElement);
        public void SaveClients() => SaveData(Clients, ClientFileName, "Clients", x => x.GetXElement);
        public void SaveImplementers() => SaveData(Implementers, ImplementerFileName, "Implementers", x => x.GetXElement);
        public void SaveShops() => SaveData(Shops, ShopFileName, "Shops", x => x.GetXElement);
        public void SaveMessages() => SaveData(Orders, ImplementerFileName, "Messages", x => x.GetXElement);

        private DataFileSingleton()
        {
            Components = LoadData(ComponentFileName, "Component", x => Component.Create(x)!)!;
            Pizzas = LoadData(PizzaFileName, "Pizza", x => Pizza.Create(x)!)!;
            Orders = LoadData(OrderFileName, "Order", x => Order.Create(x)!)!;
            Clients = LoadData(ClientFileName, "Client", x => Client.Create(x)!)!;
            Implementers = LoadData(ImplementerFileName, "Implementer", x => Implementer.Create(x)!)!;
            Shops = LoadData(ShopFileName, "Shop", x => Shop.Create(x)!)!;
            Messages = LoadData(MessageInfoFileName, "MessageInfo", x => MessageInfo.Create(x)!)!;
        }

        private static List<T>? LoadData<T>(string filename, string xmlNodeName, Func<XElement, T> selectFunction)
        {
            if (File.Exists(filename))
            {
                return XDocument.Load(filename)?.Root?.Elements(xmlNodeName)?.Select(selectFunction)?.ToList();
            }
            return new List<T>();
        }

        private static void SaveData<T>(List<T> data, string filename, string xmlNodeName, Func<T, XElement> selectFunction)
        {
            if (data != null)
            {
                new XDocument(new XElement(xmlNodeName, data.Select(selectFunction).ToArray())).Save(filename);
            }
        }
    }
}