using MySql.Data.Entity;
using System.Data.Entity;


namespace OrderService.Models
{
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
}
