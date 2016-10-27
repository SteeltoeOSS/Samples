using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class OrderItem
    {
        [Key]
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("menuItemId")]
        public long MenuItemId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public float Price { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonIgnore]
        public virtual Order Order { get; set; }
        public OrderItem()
        {
        }
    }
}
