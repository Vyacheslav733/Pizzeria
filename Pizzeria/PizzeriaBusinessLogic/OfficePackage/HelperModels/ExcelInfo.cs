using PizzeriaContracts.ViewModels;

namespace PizzeriaBusinessLogic.OfficePackage.HelperModels
{
    public class ExcelInfo : IDocument
    {
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<ReportPizzaComponentViewModel> PizzaComponents { get; set; } = new();
    }
}
