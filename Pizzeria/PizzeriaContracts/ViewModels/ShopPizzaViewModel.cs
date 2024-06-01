namespace PizzeriaContracts.ViewModels
{
    public class ShopPizzaViewModel
    {
        public ShopViewModel Shop { get; set; } = new();
        public Dictionary<int, PizzaCount> ShopPizza { get; set; } = new();
    }
}
