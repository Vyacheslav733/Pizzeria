using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaFileImplement.Models;

namespace PizzeriaFileImplement.Implements
{
    public class ImplementerStorage : IImplementerStorage
    {
        private readonly DataFileSingleton _source;
        public ImplementerStorage()
        {
            _source = DataFileSingleton.GetInstance();
        }

        public List<ImplementerViewModel> GetFullList()
        {
            return _source.Implementers.Select(x => x.GetViewModel).ToList();
        }

        public List<ImplementerViewModel> GetFilteredList(ImplementerSearchModel model)
        {
            if (model == null)
            {
                return new();
            }
            if (model.Id.HasValue)
            {
                var res = GetElement(model);
                return res != null ? new() { res } : new();
            }
            if (model.ImplementerFIO != null)
            {
                return _source.Implementers
                    .Where(x => x.ImplementerFIO.Equals(model.ImplementerFIO))
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            return new();
        }

        public ImplementerViewModel? GetElement(ImplementerSearchModel model)
        {
            if (model.Id.HasValue)
            {
                return _source.Implementers.FirstOrDefault(x => x.Id == model.Id)?.GetViewModel;
            }
            if (model.ImplementerFIO != null && model.Password != null)
            {
                return _source.Implementers.FirstOrDefault(x => x.ImplementerFIO.Equals(model.ImplementerFIO) && x.Password.Equals(model.Password))?.GetViewModel;
            }
            if (model.ImplementerFIO != null)
            {
                return _source.Implementers.FirstOrDefault(x => x.ImplementerFIO.Equals(model.ImplementerFIO))?.GetViewModel;
            }
            return null;
        }

        public ImplementerViewModel? Insert(ImplementerBindingModel model)
        {
            model.Id = _source.Implementers.Count > 0 ? _source.Implementers.Max(x => x.Id) + 1 : 1;
            var res = Implementer.Create(model);
            if (res != null)
            {
                _source.Implementers.Add(res);
                _source.SaveImplementers();
            }
            return res?.GetViewModel;
        }

        public ImplementerViewModel? Update(ImplementerBindingModel model)
        {
            var res = _source.Implementers.FirstOrDefault(x => x.Id == model.Id);
            if (res != null)
            {
                res.Update(model);
                _source.SaveImplementers();
            }
            return res?.GetViewModel;
        }

        public ImplementerViewModel? Delete(ImplementerBindingModel model)
        {
            var res = _source.Implementers.FirstOrDefault(x => x.Id == model.Id);
            if (res != null)
            {
                _source.Implementers.Remove(res);
                _source.SaveImplementers();
            }
            return res?.GetViewModel;
        }
    }
}
