using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    [Table("Stock")]
    public class Stock
    {
        public int Id { get; set; }
        public int BookId { get; set; }  // FK to Book
        public int Quantity { get; set; }

        public Book? Book { get; set; }  // Navigation
    }
}
