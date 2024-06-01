using PizzeriaBusinessLogic.OfficePackage.HelperEnums;
using PizzeriaBusinessLogic.OfficePackage.HelperModels;

namespace PizzeriaBusinessLogic.OfficePackage
{
    public abstract class AbstractSaveToPdf
    {
        public void CreateDoc(PdfInfo info)
        {
            CreatePdf(info);
            CreateParagraph(new PdfParagraph { Text = info.Title, Style = "NormalTitle", ParagraphAlignment = PdfParagraphAlignmentType.Center });
            CreateParagraph(new PdfParagraph { Text = $"с {info.DateFrom.ToShortDateString()} по {info.DateTo.ToShortDateString()}", Style = "Normal", ParagraphAlignment = PdfParagraphAlignmentType.Center });

            CreateTable(new List<string> { "2cm", "3cm", "6cm", "3cm", "3cm" });

            CreateRow(new PdfRowParameters
            {
                Texts = new List<string> { "Номер", "Дата заказа", "Пицца", "Статус", "Сумма" },
                Style = "NormalTitle",
                ParagraphAlignment = PdfParagraphAlignmentType.Center
            });

            foreach (var order in info.Orders)
            {
                CreateRow(new PdfRowParameters
                {
                    Texts = new List<string> { order.Id.ToString(), order.DateCreate.ToShortDateString(), order.PizzaName, order.Status.ToString(), order.Sum.ToString() },
                    Style = "Normal",
                    ParagraphAlignment = PdfParagraphAlignmentType.Left
                });
            }
            CreateParagraph(new PdfParagraph { Text = $"Итого: {info.Orders.Sum(x => x.Sum)}\t", Style = "Normal", ParagraphAlignment = PdfParagraphAlignmentType.Rigth });

            SavePdf(info);
        }

        public void CreateGroupedOrdersDoc(PdfGroupedOrdersInfo info)
        {
            CreatePdf(info);
            CreateParagraph(new PdfParagraph { Text = info.Title, Style = "NormalTitle", ParagraphAlignment = PdfParagraphAlignmentType.Center });

            CreateTable(new List<string> { "4cm", "3cm", "2cm" });
            CreateRow(new PdfRowParameters
            {
                Texts = new List<string> { "Дата заказа", "Кол-во", "Сумма" },
                Style = "NormalTitle",
                ParagraphAlignment = PdfParagraphAlignmentType.Center
            });


            foreach (var groupedOrder in info.GroupedOrders)
            {
                CreateRow(new PdfRowParameters
                {
                    Texts = new List<string> { groupedOrder.Date.ToShortDateString(), groupedOrder.OrdersCount.ToString(), groupedOrder.OrdersSum.ToString() },
                    Style = "Normal",
                    ParagraphAlignment = PdfParagraphAlignmentType.Left
                });
            }

            CreateParagraph(new PdfParagraph { Text = $"Итого: {info.GroupedOrders.Sum(x => x.OrdersSum)}\t", Style = "Normal", ParagraphAlignment = PdfParagraphAlignmentType.Center });

            SavePdf(info);
        }

        protected abstract void CreatePdf(IDocument info);
        protected abstract void CreateParagraph(PdfParagraph paragraph);
        protected abstract void CreateTable(List<string> columns);
        protected abstract void CreateRow(PdfRowParameters rowParameters);
        protected abstract void SavePdf(IDocument info);
    }
}
