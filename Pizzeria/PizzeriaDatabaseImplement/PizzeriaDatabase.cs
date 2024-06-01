using PizzeriaDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaDatabaseImplement
{
    public class PizzeriaDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-M2G96S06\SQLEXPRESS;Initial Catalog=PizzeriaDatabase;Integrated Security=True;MultipleActiveResultSets=True;;TrustServerCertificate=True");
            }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Component> Components { set; get; }
        public virtual DbSet<Pizza> Pizzas { set; get; }
        public virtual DbSet<PizzaComponent> PizzaComponents { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
        public virtual DbSet<Client> Clients { set; get; }
        public virtual DbSet<Implementer> Implementers { set; get; }
        public virtual DbSet<Shop> Shops { set; get; }
        public virtual DbSet<ShopPizzas> ShopPizzas { set; get; }
        public virtual DbSet<MessageInfo> MessageInfos { set; get; }
    }
}
