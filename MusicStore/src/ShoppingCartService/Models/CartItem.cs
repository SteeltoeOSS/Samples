using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartService.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        public int ItemKey { get; set; }

        public int Count { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
    }
}
