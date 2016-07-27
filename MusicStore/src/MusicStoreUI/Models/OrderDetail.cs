using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreUI.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int AlbumId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public virtual Album Album { get; set; }

        public virtual Order Order { get; set; }

        public static OrderDetail From(OrderDetailJson item)
        {
            if (item != null)
            {
                return new OrderDetail()
                {
                    OrderDetailId = item.OrderDetailId,
                    OrderId = item.OrderId,
                    AlbumId = item.ItemKey,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                  
                };
            }
            else
            {
                return new OrderDetail();
            }

        }


        public static List<OrderDetail> From(List<OrderDetailJson> items)
        {
            List<OrderDetail> results = new List<OrderDetail>();
            if (items == null)
                return results;

            foreach (var a in items)
            {
                results.Add(OrderDetail.From(a));
            }
            return results;
        }
    }
}
