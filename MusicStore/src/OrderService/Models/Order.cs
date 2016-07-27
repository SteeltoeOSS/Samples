using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OrderService.Models
{
    public class Order
    {
        [BindNever]
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public System.DateTime OrderDate { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(160)]
        public string LastName { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 3)]
        public string Address { get; set; }

        [Required]
        [StringLength(40)]
        public string City { get; set; }

        [Required]
        [StringLength(40)]
        public string State { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        [StringLength(10, MinimumLength = 5)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(40)]
        public string Country { get; set; }

        [Required]
        [StringLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public decimal Total { get; set; }

        [BindNever]
        public List<OrderDetail> OrderDetails { get; set; }

        public static Order From(OrderJson item)
        {
            var result = new Order()
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
                OrderDetails = new List<OrderDetail>()
            };

            foreach(var json in item.OrderDetails)
            {
                OrderDetail detail = OrderDetail.From(json);
                result.OrderDetails.Add(detail);
            }

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
