using PizzeriaContracts.ViewModels;

namespace PizzeriaBusinessLogic.OfficePackage.HelperModels
{
    public class WordInfo : IDocument
    {
        public string FileName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<PizzaViewModel> Pizzas { get; set; } = new();
    }
}
