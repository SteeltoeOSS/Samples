

using MySql.Data.Entity;
using System.Data.Entity;

namespace ShoppingCartService.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]

    public class ShopingCartContext : DbContext
    {
        public ShopingCartContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<ShoppingCart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

    }
}
