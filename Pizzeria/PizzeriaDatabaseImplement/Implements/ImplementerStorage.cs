using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaDatabaseImplement.Models;

namespace PizzeriaDatabaseImplement.Implements
{
    public class ImplementerStorage : IImplementerStorage
    {
        public List<ImplementerViewModel> GetFullList()
        {
            using var context = new PizzeriaDatabase();
            return context.Implementers.Select(x => x.GetViewModel).ToList();
        }

        public List<ImplementerViewModel> GetFilteredList(ImplementerSearchModel model)
        {
            if (string.IsNullOrEmpty(model.ImplementerFIO))
            {
                return new();
            }
            using var context = new PizzeriaDatabase();
            return context.Implementers.Where(x => x.ImplementerFIO.Contains(model.ImplementerFIO)).Select(x => x.GetViewModel).ToList();
        }

        public ImplementerViewModel? GetElement(ImplementerSearchModel model)
        {
            if (string.IsNullOrEmpty(model.ImplementerFIO) && !model.Id.HasValue)
            {
                return null;
            }
            using var context = new PizzeriaDatabase();
            return context.Implementers.FirstOrDefault(x =>
                (!string.IsNullOrEmpty(model.ImplementerFIO) && x.ImplementerFIO == model.ImplementerFIO && (!string.IsNullOrEmpty(model.Password) ? x.Password == model.Password : true)) ||
                (model.Id.HasValue && x.Id == model.Id))
                ?.GetViewModel;
        }

        public ImplementerViewModel? Insert(ImplementerBindingModel model)
        {
            var newImplementer = Implementer.Create(model);
            if (newImplementer == null)
            {
                return null;
            }
            using var context = new PizzeriaDatabase();
            context.Implementers.Add(newImplementer);
            context.SaveChanges();
            return newImplementer.GetViewModel;
        }

        public ImplementerViewModel? Update(ImplementerBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            var implementer = context.Implementers.FirstOrDefault(x => x.Id == model.Id);
            if (implementer == null)
            {
                return null;
            }
            implementer.Update(model);
            context.SaveChanges();
            return implementer.GetViewModel;
        }

        public ImplementerViewModel? Delete(ImplementerBindingModel model)
        {
            using var context = new PizzeriaDatabase();
            var implementer = context.Implementers.FirstOrDefault(rec => rec.Id == model.Id);
            if (implementer != null)
            {
                context.Implementers.Remove(implementer);
                context.SaveChanges();
                return implementer.GetViewModel;
            }
            return null;
        }
    }
}
