using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PizzeriaDatabaseImplement.Models
{
    [DataContract]
    public class Pizza : IPizzaModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string PizzaName { get; set; } = string.Empty;

        [DataMember]
        [Required]
        public double Price { get; set; }

        private Dictionary<int, (IComponentModel, int)>? _pizzaComponents = null;

        [DataMember]
        [NotMapped]
        public Dictionary<int, (IComponentModel, int)> PizzaComponents
        {
            get
            {
                if (_pizzaComponents == null)
                {
                    _pizzaComponents = Components.ToDictionary(recPC => recPC.ComponentId, recPC =>
                    (recPC.Component as IComponentModel, recPC.Count));
                }
                return _pizzaComponents;
            }
        }

        [ForeignKey("PizzaId")]
        public virtual List<PizzaComponent> Components { get; set; } = new();
        [ForeignKey("PizzaId")]
        public virtual List<Order> Orders { get; set; } = new();

        public static Pizza Create(PizzeriaDatabase context, PizzaBindingModel model)
        {
            return new Pizza()
            {
                Id = model.Id,
                PizzaName = model.PizzaName,
                Price = model.Price,
                Components = model.PizzaComponents.Select(x => new PizzaComponent
                {
                    Component = context.Components.First(y => y.Id == x.Key),
                    Count = x.Value.Item2
                }).ToList()
            };
        }

        public void Update(PizzaBindingModel model)
        {
            PizzaName = model.PizzaName;
            Price = model.Price;
        }

        public PizzaViewModel GetViewModel => new()
        {
            Id = Id,
            PizzaName = PizzaName,
            Price = Price,
            PizzaComponents = PizzaComponents
        };

        public void UpdateComponents(PizzeriaDatabase context, PizzaBindingModel model)
        {
            var pizzaComponents = context.PizzaComponents.Where(rec => rec.PizzaId == model.Id).ToList();

            if (pizzaComponents != null && pizzaComponents.Count > 0)
            {
                context.PizzaComponents.RemoveRange(pizzaComponents.Where(rec => !model.PizzaComponents.ContainsKey(rec.ComponentId)));
                context.SaveChanges();
                foreach (var updateComponent in pizzaComponents)
                {
                    updateComponent.Count = model.PizzaComponents[updateComponent.ComponentId].Item2;
                    model.PizzaComponents.Remove(updateComponent.ComponentId);
                }
                context.SaveChanges();
            }

            var pizza = context.Pizzas.First(x => x.Id == Id);
            foreach (var pc in model.PizzaComponents)
            {
                context.PizzaComponents.Add(new PizzaComponent
                {
                    Pizza = pizza,
                    Component = context.Components.First(x => x.Id == pc.Key),
                    Count = pc.Value.Item2
                });
                context.SaveChanges();
            }
            _pizzaComponents = null;
        }
    }
}
