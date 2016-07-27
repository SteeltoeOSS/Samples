using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MusicStoreUI.Models
{
    public class CartItem
    {
   
        public int CartItemId { get; set; }


        public string CartId { get; set; }

        public int AlbumId { get; set; }
        public int Count { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public virtual Album Album { get; set; }

        public static CartItem From(CartItemJson item)
        {
            if (item != null)
            {
                return new CartItem()
                {
                    CartItemId = item.CartItemId,
                    CartId = item.CartId,
                    AlbumId = item.ItemKey,
                    Count = item.Count,
                    DateCreated = item.DateCreated
                };
            } else
            {
                return new CartItem()
                {
                    CartItemId = 0,
                    CartId = string.Empty,
                    AlbumId = 0,
                    Count = 0,
                    DateCreated = DateTime.UtcNow
                };
            }

        }


        public static List<CartItem> From(List<CartItemJson> items)
        {
            List<CartItem> results = new List<CartItem>();
            if (items == null)
                return results;

            foreach (var a in items)
            {
                results.Add(CartItem.From(a));
            }
            return results;
        }

    }
}

