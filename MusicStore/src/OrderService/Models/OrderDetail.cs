using System.Collections.Generic;

namespace OrderService.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int ItemKey { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public virtual Order Order { get; set; }

        public static OrderDetail From(OrderDetailJson item)
        {
            if (item != null)
            {
                return new OrderDetail()
                {
                    OrderDetailId = item.OrderDetailId,
                    OrderId = item.OrderId,
                    ItemKey = item.ItemKey,
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