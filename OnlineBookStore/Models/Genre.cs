using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    [Table("Genre")]
    public class Genre
    {
        public int Id { get; set; }           // Primary Key

        [Required]
        [MaxLength(40)]
        public string GenreName { get; set; }

        // 1 to Many Relationship: 1 Genre has many Books
        public List<Book> Books { get; set; } // Navigation property (1 genre has many books)
    }
}
