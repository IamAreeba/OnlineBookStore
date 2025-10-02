using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }   // FK to Order

        [Required]
        public int BookId { get; set; }    // FK to Book

        [Required]
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public Order Order { get; set; }   // Navigation
        public Book Book { get; set; }     // Navigation
    }
}
