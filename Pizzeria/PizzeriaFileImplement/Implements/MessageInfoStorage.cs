using PizzeriaContracts.BindingModels;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;
using PizzeriaFileImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaFileImplement.Implements
{
	public class MessageInfoStorage : IMessageInfoStorage
	{
		private readonly DataFileSingleton _source;
		public MessageInfoStorage()
		{
			_source = DataFileSingleton.GetInstance();
		}

		public MessageInfoViewModel? GetElement(MessageInfoSearchModel model)
		{
			if (model.MessageId != null)
			{
				return _source.Messages.FirstOrDefault(x => x.MessageId == model.MessageId)?.GetViewModel;
			}
			return null;
		}

		public List<MessageInfoViewModel> GetFilteredList(MessageInfoSearchModel model)
		{
			var res = _source.Messages.Where(x => !model.ClientId.HasValue || x.ClientId == model.ClientId).Select(x => x.GetViewModel);
			if (!(model.PageIndex.HasValue && model.PageLength.HasValue))
			{
				return res.ToList();
			}
			return res.Skip((model.PageIndex.Value - 1) * model.PageLength.Value).Take(model.PageLength.Value).ToList();
		}

		public List<MessageInfoViewModel> GetFullList()
		{
			return _source.Messages
				.Select(x => x.GetViewModel)
				.ToList();
		}

		public MessageInfoViewModel? Insert(MessageInfoBindingModel model)
		{
			var newMessage = MessageInfo.Create(model);
			if (newMessage == null)
			{
				return null;
			}
			_source.Messages.Add(newMessage);
			_source.SaveMessages();
			return newMessage.GetViewModel;
		}

		public MessageInfoViewModel? Update(MessageInfoBindingModel model)
		{
			var res = _source.Messages.FirstOrDefault(x => x.MessageId.Equals(model.MessageId));
			if (res != null)
			{
				res.Update(model);
				_source.SaveMessages();
			}
			return res?.GetViewModel;
		}
	}
}
