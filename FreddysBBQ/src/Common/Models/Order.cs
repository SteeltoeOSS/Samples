using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class Order
    {
        [Key]
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("placed")]
        public DateTime Placed { get; set; }
        [JsonProperty("total")]
        public float Total { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("orderItems")]
        public virtual List<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
            Placed = DateTime.UtcNow;
        }

    }
}
