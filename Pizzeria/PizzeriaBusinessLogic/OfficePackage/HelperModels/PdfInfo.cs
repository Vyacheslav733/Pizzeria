using PizzeriaContracts.ViewModels;

namespace PizzeriaBusinessLogic.OfficePackage.HelperModels
{
    public class PdfInfo : IDocument
    {
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<ReportOrdersViewModel> Orders { get; set; } = new();
    }
}
