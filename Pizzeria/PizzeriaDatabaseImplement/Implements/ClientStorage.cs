using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaDatabaseImplement.Models;

namespace PizzeriaDatabaseImplement.Implements
{
	public class ClientStorage : IClientStorage
	{
		public List<ClientViewModel> GetFullList()
		{
			using var context = new PizzeriaDatabase();
			return context.Clients.Select(x => x.GetViewModel).ToList();
		}

		public List<ClientViewModel> GetFilteredList(ClientSearchModel model)
		{
			if (string.IsNullOrEmpty(model.ClientFIO))
			{
				return new();
			}
			using var context = new PizzeriaDatabase();
			return context.Clients.Where(x => x.ClientFIO.Contains(model.ClientFIO)).Select(x => x.GetViewModel).ToList();
		}

		public ClientViewModel? GetElement(ClientSearchModel model)
		{
			if (string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(model.Password) && !model.Id.HasValue)
			{
				return null;
			}
			using var context = new PizzeriaDatabase();
			return context.Clients.FirstOrDefault(x => (model.Id.HasValue && x.Id == model.Id) ||
				 (!string.IsNullOrEmpty(model.ClientFIO) && x.ClientFIO == model.ClientFIO) ||
				 (!string.IsNullOrEmpty(model.Email) && x.Email == model.Email && (string.IsNullOrEmpty(model.Password) || x.Password == model.Password)))?.GetViewModel;
		}

		public ClientViewModel? Insert(ClientBindingModel model)
		{
			var newComponent = Client.Create(model);
			if (newComponent == null)
			{
				return null;
			}
			using var context = new PizzeriaDatabase();
			context.Clients.Add(newComponent);
			context.SaveChanges();
			return newComponent.GetViewModel;
		}

		public ClientViewModel? Update(ClientBindingModel model)
		{
			using var context = new PizzeriaDatabase();
			var component = context.Clients.FirstOrDefault(x => x.Id == model.Id);
			if (component == null)
			{
				return null;
			}
			component.Update(model);
			context.SaveChanges();
			return component.GetViewModel;
		}

		public ClientViewModel? Delete(ClientBindingModel model)
		{
			using var context = new PizzeriaDatabase();
			var element = context.Clients.FirstOrDefault(rec => rec.Id == model.Id);
			if (element != null)
			{
				context.Clients.Remove(element);
				context.SaveChanges();
				return element.GetViewModel;
			}
			return null;
		}
	}
}
