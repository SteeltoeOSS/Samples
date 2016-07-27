using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Entity;

namespace OrderService.Models
{
    public static class SampleData
    {
   
        public static async Task InitializeOrderDatabaseAsync(IServiceProvider serviceProvider)
        {

            Database.SetInitializer<OrdersContext>(new DropCreateDatabaseAlways<OrdersContext>());
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<OrdersContext>();
                await InsertTestData(serviceProvider);
            }
        }

        private static async Task InsertTestData(IServiceProvider serviceProvider)
        {
            var orders = GetOrders(Details);
            await AddOrUpdateAsync(serviceProvider, o => o.OrderId, orders);
        }

        // TODO [EF] This may be replaced by a first class mechanism in EF
        private static async Task AddOrUpdateAsync<TEntity>(
            IServiceProvider serviceProvider,
            Func<TEntity, object> propertyToMatch, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            // Query in a separate context so that we can attach existing entities as modified
            List<TEntity> existingData;
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<OrdersContext>();
                existingData = db.Set<TEntity>().ToList();
            }

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<OrdersContext>();
                foreach (var item in entities)
                {
                    db.Entry(item).State = existingData.Any(g => propertyToMatch(g).Equals(propertyToMatch(item)))
                        ? EntityState.Modified
                        : EntityState.Added;
                }

                await db.SaveChangesAsync();
            }
        }

        private static Order[] GetOrders(Dictionary<int, OrderDetail> orderDetails)
        {
            var orders = new Order[0];
            return orders;
        }

        public static Dictionary<int, OrderDetail> Details { get; set; } = new Dictionary<int, OrderDetail>();
    
        }
}


