
using MySql.Data.Entity;
using System.Data.Entity;
using Common.Models;

namespace OrderService.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class OrderContext : DbContext
    {
        public OrderContext(string connectionString)
            : base(connectionString)
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}
