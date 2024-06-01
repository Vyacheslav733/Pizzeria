using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PizzeriaFileImplement.Models
{
    [DataContract]
    public class MessageInfo : IMessageInfoModel
    {
        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string MessageId { get; private set; } = string.Empty;

        [DataMember]
        public int? ClientId { get; private set; }

        [DataMember]
        public string SenderName { get; private set; } = string.Empty;

        [DataMember]
        public DateTime DateDelivery { get; private set; } = DateTime.Now;

        [DataMember]
        public string Subject { get; private set; } = string.Empty;

        [DataMember]
        public string Body { get; private set; } = string.Empty;

        public bool IsReaded { get; private set; }
        public bool IsReply { get; private set; }
        public string? ReplyMessageId { get; private set; } = string.Empty;

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

		public static MessageInfo? Create(XElement element)
		{
			if (element == null)
			{
				return null;
			}
			return new()
			{
				Body = element.Attribute("Body")!.Value,
				IsReply = Convert.ToBoolean(element.Attribute("IsReply")!.Value),
				IsReaded = Convert.ToBoolean(element.Attribute("HasRead")!.Value),
				Subject = element.Attribute("Subject")!.Value,
				ClientId = Convert.ToInt32(element.Attribute("ClientId")!.Value),
				MessageId = element.Attribute("MessageId")!.Value,
				ReplyMessageId = element.Attribute("ReplyMessageId")!.Value,
				SenderName = element.Attribute("SenderName")!.Value,
				DateDelivery = Convert.ToDateTime(element.Attribute("DateDelivery")!.Value),
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

		public XElement GetXElement => new("MessageInfo",
			new XAttribute("Body", Body),
			new XAttribute("IsReply", IsReply),
			new XAttribute("IsReaded", IsReaded),
			new XAttribute("Subject", Subject),
			new XAttribute("ClientId", ClientId),
			new XAttribute("MessageId", MessageId),
			new XAttribute("ReplyMessageId", ReplyMessageId),
			new XAttribute("SenderName", SenderName),
			new XAttribute("DateDelivery", DateDelivery)
			);
	}
}
