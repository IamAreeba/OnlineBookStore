using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    [Table("OrderStatus")]
    public class OrderStatus
    {
        public int Id { get; set; }        // PK

        [Required]
        public int StatusId { get; set; }  // optional extra id

        [Required, MaxLength(20)]
        public string? StatusName { get; set; }
    }
}
