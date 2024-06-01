using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaFileImplement.Models;

namespace PizzeriaFileImplement.Implements
{
    public class ComponentStorage : IComponentStorage
    {
        private readonly DataFileSingleton source;

        public ComponentStorage()
        {
            source = DataFileSingleton.GetInstance();
        }

        public List<ComponentViewModel> GetFullList()
        {
            return source.Components.Select(x => x.GetViewModel).ToList();
        }

        public List<ComponentViewModel> GetFilteredList(ComponentSearchModel model)
        {
            if (string.IsNullOrEmpty(model.ComponentName))
            {
                return new();
            }
            return source.Components.Where(x => x.ComponentName.Contains(model.ComponentName)).Select(x => x.GetViewModel).ToList();
        }

        public ComponentViewModel? GetElement(ComponentSearchModel model)
        {
            if (string.IsNullOrEmpty(model.ComponentName) && !model.Id.HasValue)
            {
                return null;
            }
            return source.Components.FirstOrDefault(x =>
            (!string.IsNullOrEmpty(model.ComponentName) && x.ComponentName == model.ComponentName) ||
            (model.Id.HasValue && x.Id == model.Id))
                ?.GetViewModel;
        }

        public ComponentViewModel? Insert(ComponentBindingModel model)
        {
            model.Id = source.Components.Count > 0 ? source.Components.Max(x => x.Id) + 1 : 1;
            var newComponent = Component.Create(model);
            if (newComponent == null)
            {
                return null;
            }
            source.Components.Add(newComponent);
            source.SaveComponents();
            return newComponent.GetViewModel;
        }

        public ComponentViewModel? Update(ComponentBindingModel model)
        {
            var component = source.Components.FirstOrDefault(x => x.Id == model.Id);
            if (component == null)
            {
                return null;
            }
            component.Update(model);
            source.SaveComponents();
            return component.GetViewModel;
        }

        public ComponentViewModel? Delete(ComponentBindingModel model)
        {
            var element = source.Components.FirstOrDefault(x => x.Id == model.Id);
            if (element != null)
            {
                source.Components.Remove(element);
                source.SaveComponents();
                return element.GetViewModel;
            }
            return null;
        }
    }
}
