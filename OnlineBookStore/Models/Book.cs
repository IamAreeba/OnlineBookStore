using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    [Table("Book")]

    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? BookName { get; set; }
        public string? AuthorName { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }

        [Required]
        public int GenreId { get; set; }    // FK to Genre table
        public Genre Genre { get; set; }    // Navigation property (to access genre info)

        public List<OrderDetail> OrderDetail { get; set; }  // 1 book can be in many orders
        public List<CartDetail> CartDetail { get; set; }    // 1 book can be in many cart items
        public Stock Stock { get; set; }                   // 1 book has 1 stock record

        [NotMapped]
        public string GenreName { get; set; }   // Not in DB, used for display

        [NotMapped]
        public int Quantity { get; set; }       // Not in DB, used for display
    }
}
