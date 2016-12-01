
using Microsoft.EntityFrameworkCore;

namespace OrderService.Models
{

    public class OrdersContext : DbContext
    {
        public OrdersContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }

}
