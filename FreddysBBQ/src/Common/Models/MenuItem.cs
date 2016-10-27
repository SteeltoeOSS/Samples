using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
namespace Common.Models
{
    public class MenuItem
    {
        [Key]
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public float Price { get; set; }
        public MenuItem()
        {
            Id = -1;
        }
    }
}