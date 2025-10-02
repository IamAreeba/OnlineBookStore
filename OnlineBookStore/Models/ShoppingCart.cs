﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }  // Owner of cart
        public ICollection<CartDetail> CartDetails { get; set; } // 1 cart has many cart items
    }
}
