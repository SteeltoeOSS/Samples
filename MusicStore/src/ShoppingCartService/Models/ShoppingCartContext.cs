
using Microsoft.EntityFrameworkCore;


namespace ShoppingCartService.Models
{

    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ShoppingCart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

    }

}
