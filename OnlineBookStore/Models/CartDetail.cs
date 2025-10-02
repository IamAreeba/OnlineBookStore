using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }

        [Required]
        public int ShoppingCartId { get; set; } // FK to ShoppingCart
        public int BookId { get; set; }         // FK to Book
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public Book Book { get; set; }          // Navigation
        public ShoppingCart ShoppingCart { get; set; } // Navigation
    }
}
