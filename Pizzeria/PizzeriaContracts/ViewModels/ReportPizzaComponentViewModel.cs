namespace PizzeriaContracts.ViewModels
{
    public class ReportPizzaComponentViewModel
    {
        public string PizzaName { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public List<(string Component, int Count)> Components { get; set; } = new();
    }
}