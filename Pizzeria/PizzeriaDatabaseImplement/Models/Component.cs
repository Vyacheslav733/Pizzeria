using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PizzeriaDataModels.Models;
using System.Runtime.Serialization;

namespace PizzeriaDatabaseImplement.Models
{
    [DataContract]
    public class Component : IComponentModel
    {
        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        [Required]
        public string ComponentName { get; private set; } = string.Empty;

        [DataMember]
        [Required]
        public double Cost { get; set; }

        [ForeignKey("ComponentId")]
        public virtual List<PizzaComponent> PizzaComponents { get; set; } = new();

        public static Component? Create(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Component()
            {
                Id = model.Id,
                ComponentName = model.ComponentName,
                Cost = model.Cost
            };
        }

        public static Component Create(ComponentViewModel model)
        {
            return new Component
            {
                Id = model.Id,
                ComponentName = model.ComponentName,
                Cost = model.Cost
            };
        }

        public void Update(ComponentBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            ComponentName = model.ComponentName;
            Cost = model.Cost;
        }

        public ComponentViewModel GetViewModel => new()
        {
            Id = Id,
            ComponentName = ComponentName,
            Cost = Cost
        };
    }
}
