using PizzeriaDataModels.Models;

namespace PizzeriaContracts.BindingModels
{
    public class MessageInfoBindingModel : IMessageInfoModel
    {
        public string MessageId { get; set; } = string.Empty;
        public int? ClientId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime DateDelivery { get; set; }
        public bool IsReaded { get; set; }
        public string? ReplyMessageId { get; set; }
        public bool IsReply { get; set; }
        public int Id => throw new NotImplementedException();
    }
}
