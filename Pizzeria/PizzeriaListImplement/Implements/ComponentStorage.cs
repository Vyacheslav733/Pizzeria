using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaListImplement.Implements
{
    public class ComponentStorage : IComponentStorage
    {
        private readonly DataListSingleton _source;

        public ComponentStorage()
        {
            _source = DataListSingleton.GetInstance();
        }

        public List<ComponentViewModel> GetFullList()
        {
            var result = new List<ComponentViewModel>();
            foreach (var component in _source.Components)
            {
                result.Add(component.GetViewModel);
            }
            return result;
        }

        public List<ComponentViewModel> GetFilteredList(ComponentSearchModel model)
        {
            var result = new List<ComponentViewModel>();
            if (string.IsNullOrEmpty(model.ComponentName))
            {
                return result;
            }
            foreach (var component in _source.Components)
            {
                if (component.ComponentName.Contains(model.ComponentName))
                {
                    result.Add(component.GetViewModel);
                }
            }
            return result;
        }

        public ComponentViewModel? GetElement(ComponentSearchModel model)
        {
            if (string.IsNullOrEmpty(model.ComponentName) && !model.Id.HasValue)
            {
                return null;
            }
            foreach (var component in _source.Components)
            {
                if ((!string.IsNullOrEmpty(model.ComponentName) && component.ComponentName == model.ComponentName) ||
                    (model.Id.HasValue && component.Id == model.Id))
                {
                    return component.GetViewModel;
                }
            }
            return null;
        }

        public ComponentViewModel? Insert(ComponentBindingModel model)
        {
            model.Id = 1;
            foreach (var component in _source.Components)
            {
                if (model.Id <= component.Id)
                {
                    model.Id = component.Id + 1;
                }
            }
            var newComponent = Component.Create(model);
            if (newComponent == null)
            {
                return null;
            }
            _source.Components.Add(newComponent);
            return newComponent.GetViewModel;
        }

        public ComponentViewModel? Update(ComponentBindingModel model)
        {
            foreach (var component in _source.Components)
            {
                if (component.Id == model.Id)
                {
                    component.Update(model);
                    return component.GetViewModel;
                }
            }
            return null;
        }

        public ComponentViewModel? Delete(ComponentBindingModel model)
        {
            for (int i = 0; i < _source.Components.Count; ++i)
            {
                if (_source.Components[i].Id == model.Id)
                {
                    var element = _source.Components[i];
                    _source.Components.RemoveAt(i);
                    return element.GetViewModel;
                }
            }
            return null;
        }
    }
}
