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
	public class MessageInfoStorage : IMessageInfoStorage
	{
		private readonly DataListSingleton _source;
		public MessageInfoStorage()
		{
			_source = DataListSingleton.GetInstance();
		}

		public MessageInfoViewModel? GetElement(MessageInfoSearchModel model)
		{
			foreach (var message in _source.Messages)
			{
				if (model.MessageId != null && model.MessageId.Equals(message.MessageId))
					return message.GetViewModel;
			}
			return null;
		}

		public List<MessageInfoViewModel> GetFilteredList(MessageInfoSearchModel model)
		{
			List<MessageInfoViewModel> result = new();
			foreach (var item in _source.Messages)
			{
				if (item.ClientId.HasValue && item.ClientId == model.ClientId)
				{
					result.Add(item.GetViewModel);
				}
			}

			if (!(model.PageIndex.HasValue && model.PageLength.HasValue))
			{
				return result;
			}
			if (model.PageIndex * model.PageLength >= result.Count)
			{
				return null;
			}
			List<MessageInfoViewModel> filteredResult = new();
			for (var i = (model.PageIndex.Value - 1) * model.PageLength.Value; i < model.PageIndex.Value * model.PageLength.Value; i++)
			{
				filteredResult.Add(result[i]);
			}
			return filteredResult;
		}

		public List<MessageInfoViewModel> GetFullList()
		{
			List<MessageInfoViewModel> result = new();
			foreach (var item in _source.Messages)
			{
				result.Add(item.GetViewModel);
			}
			return result;
		}

		public MessageInfoViewModel? Insert(MessageInfoBindingModel model)
		{
			var newMessage = MessageInfo.Create(model);
			if (newMessage == null)
			{
				return null;
			}
			_source.Messages.Add(newMessage);
			return newMessage.GetViewModel;
		}

		public MessageInfoViewModel? Update(MessageInfoBindingModel model)
		{
			foreach (var message in _source.Messages)
			{
				if (message.MessageId.Equals(model.MessageId))
				{
					message.Update(model);
					return message.GetViewModel;
				}
			}
			return null;
		}
	}
}
