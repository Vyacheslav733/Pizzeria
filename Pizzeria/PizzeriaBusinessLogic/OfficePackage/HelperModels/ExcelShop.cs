using PizzeriaContracts.ViewModels;

namespace PizzeriaBusinessLogic.OfficePackage.HelperModels
{
    public class ExcelShop : IDocument
    {
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<ReportShopsViewModel> ShopPizzas { get; set; } = new();
    }
}
