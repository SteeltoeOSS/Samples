using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class OrderDetailJson
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int ItemKey { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public static OrderDetailJson From(OrderDetail item)
        {
            var result = new OrderDetailJson()
            {
                OrderDetailId = item.OrderDetailId,
                OrderId = item.OrderId,
                ItemKey = item.ItemKey,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            };

            return result;
        }

        public static List<OrderDetailJson> From(List<OrderDetail> items)
        {
            List<OrderDetailJson> results = new List<OrderDetailJson>();
            if (items == null)
                return results;

            foreach (var a in items)
            {
                results.Add(OrderDetailJson.From(a));
            }
            return results;
        }

    }
}
