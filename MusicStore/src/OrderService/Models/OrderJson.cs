using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class OrderJson
    {
        public int OrderId { get; set; }

        public System.DateTime OrderDate { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
        public decimal Total { get; set; }
        public List<OrderDetailJson> OrderDetails { get; set; }

        public static OrderJson From(Order item)
        {
            var result = new OrderJson()
            {
                OrderId = item.OrderId,
                OrderDate = item.OrderDate,
                Username = item.Username,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Address = item.Address,
                City = item.City,
                State = item.State,
                PostalCode = item.PostalCode,
                Country = item.Country,
                Phone = item.Phone,
                Email = item.Email,
                Total = item.Total,
                OrderDetails = OrderDetailJson.From(item.OrderDetails)

            };

            return result;
        }

        public static List<OrderJson> From(List<Order> items)
        {
            List<OrderJson> results = new List<OrderJson>();
            if (items == null)
                return results;

            foreach (var a in items)
            {
                results.Add(OrderJson.From(a));
            }
            return results;
        }
    }
}
