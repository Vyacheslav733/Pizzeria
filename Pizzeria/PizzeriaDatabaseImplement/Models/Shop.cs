using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PizzeriaDatabaseImplement.Models
{
    [DataContract]
    public class Shop : IShopModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string ShopName { get; set; } = string.Empty;

        [DataMember]
        [Required]
        public string Adress { get; set; } = string.Empty;

        [DataMember]
        [Required]
        public DateTime OpeningDate { get; set; }

        [DataMember]
        [Required]
        public int PizzaMaxCount { get; set; }

        private Dictionary<int, (IPizzaModel, int)>? _shopPizzas = null;

        public Dictionary<int, (IPizzaModel, int)> ShopPizzas
        {
            get
            {
                if (_shopPizzas == null)
                {
                    if (_shopPizzas == null)
                    {
                        _shopPizzas = Pizzas
                                .ToDictionary(recSP => recSP.PizzaId, recSP => (recSP.Pizza as IPizzaModel, recSP.Count));
                    }
                    return _shopPizzas;
                }
                return _shopPizzas;
            }
        }

        [ForeignKey("ShopId")]
        public List<ShopPizzas> Pizzas { get; set; } = new();

        public static Shop Create(PizzeriaDatabase context, ShopBindingModel model)
        {
            return new Shop()
            {
                Id = model.Id,
                ShopName = model.ShopName,
                Adress = model.Adress,
                OpeningDate = model.OpeningDate,
                Pizzas = model.ShopPizzas.Select(x => new ShopPizzas
                {
                    Pizza = context.Pizzas.First(y => y.Id == x.Key),
                    Count = x.Value.Item2
                }).ToList(),
                PizzaMaxCount = model.PizzaMaxCount
            };
        }

        public void Update(ShopBindingModel model)
        {
            ShopName = model.ShopName;
            Adress = model.Adress;
            OpeningDate = model.OpeningDate;
            PizzaMaxCount = model.PizzaMaxCount;
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

        public void UpdatePizzas(PizzeriaDatabase context, ShopBindingModel model)
        {
            var ShopPizzas = context.ShopPizzas.Where(rec => rec.ShopId == model.Id).ToList();
            if (ShopPizzas != null && ShopPizzas.Count > 0)
            {
                context.ShopPizzas.RemoveRange(ShopPizzas.Where(rec => !model.ShopPizzas.ContainsKey(rec.PizzaId)));
                context.SaveChanges();
                ShopPizzas = context.ShopPizzas.Where(rec => rec.ShopId == model.Id).ToList();
                foreach (var updatePizza in ShopPizzas)
                {
                    updatePizza.Count = model.ShopPizzas[updatePizza.PizzaId].Item2;
                    model.ShopPizzas.Remove(updatePizza.PizzaId);
                }
                context.SaveChanges();
            }
            var shop = context.Shops.First(x => x.Id == Id);
            foreach (var ar in model.ShopPizzas)
            {
                context.ShopPizzas.Add(new ShopPizzas
                {
                    Shop = shop,
                    Pizza = context.Pizzas.First(x => x.Id == ar.Key),
                    Count = ar.Value.Item2
                });
                context.SaveChanges();
            }
            _shopPizzas = null;
        }

        public void PizzasDictionatyUpdate(PizzeriaDatabase context)
        {
            UpdatePizzas(context, new ShopBindingModel
            {
                Id = Id,
                ShopPizzas = ShopPizzas,
            });
        }
    }
}
