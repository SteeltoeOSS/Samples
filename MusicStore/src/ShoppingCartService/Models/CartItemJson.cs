using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartService.Models
{
    public class CartItemJson
    {
   
        public int CartItemId { get; set; }
        public string CartId { get; set; }

        public int ItemKey { get; set; }
        public int Count { get; set; }

        public DateTime DateCreated { get; set; }

        public static CartItemJson From(CartItem item)
        {
            var result = new CartItemJson()
            {
                CartItemId = item.CartItemId,
                CartId = item.CartId,
                ItemKey = item.ItemKey,
                Count = item.Count,
                DateCreated = item.DateCreated
            };

            return result;
        }

        public static List<CartItemJson> From(List<CartItem> items)
        {
            List<CartItemJson> results = new List<CartItemJson>();
            if (items == null)
                return results;

            foreach (var a in items)
            {
                results.Add(CartItemJson.From(a));
            }
            return results;
        }
    }
}
