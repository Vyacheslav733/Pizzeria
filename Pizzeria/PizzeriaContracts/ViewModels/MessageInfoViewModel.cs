using PizzeriaContracts.Attributes;
using PizzeriaDataModels.Models;

namespace PizzeriaContracts.ViewModels
{
	public class MessageInfoViewModel : IMessageInfoModel
	{
        [Column(visible: false)]
        public int Id { get; set; }

        [Column(visible: false)]
        public string MessageId { get; set; } = string.Empty;

        [Column(visible: false)]
        public int? ClientId { get; set; }

        [Column(title: "Отправитель", width: 150)]
        public string SenderName { get; set; } = string.Empty;

        [Column(title: "Дата письма", width: 120)]
        public DateTime DateDelivery { get; set; }

        [Column(title: "Заголовок", width: 120)]
        public string Subject { get; set; } = string.Empty;

        [Column(title: "Текст", gridViewAutoSize: GridViewAutoSize.Fill, isUseAutoSize: true)]
        public string Body { get; set; } = string.Empty;

        [Column("Прочитанно", width: 120)]
        public bool IsReaded { get; set; }

        public string? ReplyMessageId { get; set; }

        public IMessageInfoModel? Reply { get; set; }

        public bool IsReply { get; set; }
    }
}
