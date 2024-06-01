using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaListImplement.Models
{
	public class MessageInfo : IMessageInfoModel
	{
		public string MessageId { get; private set; } = string.Empty;

		public int? ClientId { get; private set; }

		public string SenderName { get; private set; } = string.Empty;

		public DateTime DateDelivery { get; private set; } = DateTime.Now;

		public string Subject { get; private set; } = string.Empty;

		public string Body { get; private set; } = string.Empty;
		public bool IsReaded { get; private set; }
		public bool IsReply { get; private set; }
		public string? ReplyMessageId { get; private set; }

        public int Id => throw new NotImplementedException();

		public static MessageInfo? Create(MessageInfoBindingModel model)
		{
			if (model == null)
			{
				return null;
			}
			return new()
			{
				Body = model.Body,
				IsReply = model.IsReply,
				IsReaded = model.IsReaded,
				Subject = model.Subject,
				ClientId = model.ClientId,
				MessageId = model.MessageId,
				SenderName = model.SenderName,
				DateDelivery = model.DateDelivery,
				ReplyMessageId = model.ReplyMessageId,
			};
		}

		public void Update(MessageInfoBindingModel model)
		{
			if (model == null)
			{
				return;
			}
			IsReply = model.IsReply;
			IsReaded = model.IsReaded;
		}

		public MessageInfoViewModel GetViewModel => new()
		{
			Body = Body,
			IsReply = IsReply,
			IsReaded = IsReaded,
			Subject = Subject,
			ClientId = ClientId,
			MessageId = MessageId,
			SenderName = SenderName,
			DateDelivery = DateDelivery,
			ReplyMessageId = ReplyMessageId,
		};
	}
}
