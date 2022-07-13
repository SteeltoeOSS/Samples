namespace Product.API.Models.Requests
{
    public record AddProductRequestDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
