
#if NET451 && MYSQL
using MySql.Data.Entity;
using System.Data.Entity;
#endif

#if !NET451 || POSTGRES
using Microsoft.EntityFrameworkCore;
#endif

namespace ShoppingCartService.Models
{
#if NET451 && MYSQL
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
#endif
#if !NET451 || POSTGRES
    public class ShopingCartContext : DbContext
    {
        public ShopingCartContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ShoppingCart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

    }
#endif
}
