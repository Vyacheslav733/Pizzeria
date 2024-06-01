namespace PizzeriaRestApi
{
    public class APIConfig
    {
        public static string? ShopPassword;

        public static void LoadData(IConfiguration configuration)
        {
            ShopPassword = configuration["ShopAPIPassword"];
        }
    }
}
    