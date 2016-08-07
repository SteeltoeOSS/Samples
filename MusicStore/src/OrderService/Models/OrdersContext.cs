#if NET451 && MYSQL
using MySql.Data.Entity;
using System.Data.Entity;
#endif

#if !NET451 || POSTGRES
using Microsoft.EntityFrameworkCore;
#endif



namespace OrderService.Models
{
#if NET451 && MYSQL
    [DbConfigurationType(typeof(MySqlEFConfiguration))]

    public class OrdersContext : DbContext
    {
        public OrdersContext(string connectionString)
            : base(connectionString)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
#endif

#if !NET451 || POSTGRES
    public class OrdersContext : DbContext
    {
        public OrdersContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
#endif

}
