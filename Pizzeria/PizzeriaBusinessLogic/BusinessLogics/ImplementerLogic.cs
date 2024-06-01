using Microsoft.Extensions.Logging;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaBusinessLogic.BusinessLogics
{
    public class ImplementerLogic : IImplementerLogic
    {
        private readonly ILogger _logger;
        private readonly IImplementerStorage _implementerStorage;

        public ImplementerLogic(ILogger<IImplementerLogic> logger, IImplementerStorage implementerStorage)
        {
            _logger = logger;
            _implementerStorage = implementerStorage;
        }

        public List<ImplementerViewModel>? ReadList(ImplementerSearchModel? model)
        {
            _logger.LogInformation("ReadList. ImplementerFIO:{ImplementerFIO}.Password:{Password}.Id:{ Id}", model?.ImplementerFIO, model?.Password?.Length, model?.Id);
            var list = model == null ? _implementerStorage.GetFullList() : _implementerStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public ImplementerViewModel? ReadElement(ImplementerSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. ImplementerFIO:{ImplementerFIO}.Password:{Password}.Id:{ Id}", model?.ImplementerFIO, model?.Password?.Length, model?.Id);
            var element = _implementerStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public bool Create(ImplementerBindingModel model)
        {
            CheckModel(model);
            if (_implementerStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Update(ImplementerBindingModel model)
        {
            CheckModel(model);
            if (_implementerStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(ImplementerBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_implementerStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(ImplementerBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.ImplementerFIO))
            {
                throw new ArgumentNullException("Нет ФИО исполнителя", nameof(model.ImplementerFIO));
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                throw new ArgumentNullException("Нет пароля исполнителя", nameof(model.Password));
            }
            if (model.WorkExperience < 0)
            {
                throw new ArgumentNullException("Стаж должен быть больше 0", nameof(model.WorkExperience));
            }
            if (model.Qualification < 0)
            {
                throw new ArgumentNullException("Квалификация должна быть положительной", nameof(model.Qualification));
            }
            _logger.LogInformation("Implementer. ImplementerFIO:{ImplementerFIO}.Password:{Password}.WorkExperience:{WorkExperience}.Qualification:{Qualification}.Id: { Id}",
                model.ImplementerFIO, model.Password, model.WorkExperience, model.Qualification, model.Id);
            var element = _implementerStorage.GetElement(new ImplementerSearchModel
            {
                ImplementerFIO = model.ImplementerFIO
            });
            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Исполнитель с таким ФИО уже есть");
            }
        }
    }
}
