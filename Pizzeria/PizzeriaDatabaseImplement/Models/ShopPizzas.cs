using System.ComponentModel.DataAnnotations;

namespace PizzeriaDatabaseImplement.Models
{
    public class ShopPizzas
    {
        public int Id { get; set; }

        [Required]
        public int PizzaId { get; set; }

        [Required]
        public int ShopId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Shop Shop { get; set; } = new();

        public virtual Pizza Pizza { get; set; } = new();
    }
}
