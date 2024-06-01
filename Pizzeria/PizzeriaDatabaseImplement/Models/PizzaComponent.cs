using System.ComponentModel.DataAnnotations;

namespace PizzeriaDatabaseImplement.Models
{
    public class PizzaComponent
    {
        public int Id { get; set; }
        [Required]
        public int PizzaId { get; set; }
        [Required]
        public int ComponentId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Component Component { get; set; } = new();
        public virtual Pizza Pizza { get; set; } = new();
    }
}
