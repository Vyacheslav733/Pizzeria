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
	public class ClientStorage : IClientStorage
	{
		private readonly DataListSingleton _source;

		public ClientStorage()
		{
			_source = DataListSingleton.GetInstance();
		}

		public List<ClientViewModel> GetFullList()
		{
			var result = new List<ClientViewModel>();
			foreach (var client in _source.Clients)
			{
				result.Add(client.GetViewModel);
			}
			return result;
		}

		public List<ClientViewModel> GetFilteredList(ClientSearchModel model)
		{
			var result = new List<ClientViewModel>();
			if (string.IsNullOrEmpty(model.ClientFIO))
			{
				return result;
			}
			foreach (var client in _source.Clients)
			{
				if (client.ClientFIO.Contains(model.ClientFIO))
				{
					result.Add(client.GetViewModel);
				}
			}
			return result;
		}

		public ClientViewModel? GetElement(ClientSearchModel model)
		{
			foreach (var client in _source.Clients)
			{
				if (model.Id.HasValue && model.Id == client.Id)
					return client.GetViewModel;
				if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password) &&
					client.Email.Equals(model.Email) && client.Password.Equals(model.Password))
					return client.GetViewModel;
				if (!string.IsNullOrEmpty(model.Email) && client.Email.Equals(model.Email))
					return client.GetViewModel;
			}
			return null;
		}

		public ClientViewModel? Insert(ClientBindingModel model)
		{
			model.Id = 1;
			foreach (var client in _source.Clients)
			{
				if (model.Id <= client.Id)
				{
					model.Id = client.Id + 1;
				}
			}
			var newClient = Client.Create(model);
			if (newClient == null)
			{
				return null;
			}
			_source.Clients.Add(newClient);
			return newClient.GetViewModel;
		}

		public ClientViewModel? Update(ClientBindingModel model)
		{
			foreach (var client in _source.Clients)
			{
				if (client.Id == model.Id)
				{
					client.Update(model);
					return client.GetViewModel;
				}
			}
			return null;
		}

		public ClientViewModel? Delete(ClientBindingModel model)
		{
			for (int i = 0; i < _source.Clients.Count; ++i)
			{
				if (_source.Clients[i].Id == model.Id)
				{
					var element = _source.Clients[i];
					_source.Clients.RemoveAt(i);
					return element.GetViewModel;
				}
			}
			return null;
		}
	}
}
