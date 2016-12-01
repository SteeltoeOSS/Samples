
using Microsoft.EntityFrameworkCore;


namespace ShoppingCartService.Models
{

    public class ShopingCartContext : DbContext
    {
        public ShopingCartContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ShoppingCart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

    }

}
