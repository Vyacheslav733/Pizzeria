using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;
using PizzeriaDataModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PizzeriaDatabaseImplement.Models
{
	public class MessageInfo : IMessageInfoModel
	{
        [NotMapped]
        public int Id { get; private set; }

        [DataMember]
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string MessageId { get; set; } = string.Empty;

        [DataMember]
        public int? ClientId { get; set; }

        public virtual Client? Client { get; set; }

        [DataMember]
        [Required]
		public string SenderName { get; set; } = string.Empty;

        [DataMember]
        [Required]
		public DateTime DateDelivery { get; set; }

        [DataMember]
        [Required]
		public string Subject { get; set; } = string.Empty;

        [DataMember]
        [Required]
		public string Body { get; set; } = string.Empty;

        [Required]
        public bool IsReaded { get; set; }

        public string? ReplyMessageId { get; set; }

        [ForeignKey("ReplyMessageId")]
        public virtual MessageInfo? Reply { get; set; }

        [Required]
        public bool IsReply { get; set; }

        public static MessageInfo? Create(MessageInfoBindingModel? model)
        {
            if (model == null)
            {
                return null;
            }
            return new()
            {
                MessageId = model.MessageId,
                ClientId = model.ClientId,
                SenderName = model.SenderName,
                DateDelivery = model.DateDelivery,
                Subject = model.Subject,
                Body = model.Body,
                IsReaded = model.IsReaded,
                ReplyMessageId = model.ReplyMessageId,
                IsReply = model.IsReply
            };
        }

        public void Update(PizzeriaDatabase context, MessageInfoBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            IsReaded = model.IsReaded;
            ReplyMessageId = model.ReplyMessageId;
            if (!string.IsNullOrEmpty(ReplyMessageId))
            {
                Reply = context.MessageInfos.First(x => x.MessageId == ReplyMessageId);
            }
        }

        public MessageInfoViewModel GetViewModel => new()
        {
            MessageId = MessageId,
            ClientId = ClientId,
            SenderName = SenderName,
            DateDelivery = DateDelivery,
            Subject = Subject,
            Body = Body,
            IsReaded = IsReaded,
            ReplyMessageId = ReplyMessageId,
            Reply = Reply,
            IsReply = IsReply
        };
    }
}
