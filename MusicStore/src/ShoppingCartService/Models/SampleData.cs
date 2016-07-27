using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Entity;

namespace ShoppingCartService.Models
{
    public static class SampleData
    {
        public static async Task InitializeShoppingCartDatabaseAsync(IServiceProvider serviceProvider)
        {

            Database.SetInitializer<ShopingCartContext>(new DropCreateDatabaseAlways<ShopingCartContext>());
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ShopingCartContext>();
                await InsertTestData(serviceProvider);
            }
        }

        private static async Task InsertTestData(IServiceProvider serviceProvider)
        {
            var carts = GetCarts(CartItems);
            await AddOrUpdateAsync(serviceProvider, c => c.CartId, carts);
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
                var db = serviceScope.ServiceProvider.GetService<ShopingCartContext>();
                existingData = db.Set<TEntity>().ToList();
            }

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ShopingCartContext>();
                foreach (var item in entities)
                {
                    db.Entry(item).State = existingData.Any(g => propertyToMatch(g).Equals(propertyToMatch(item)))
                        ? EntityState.Modified
                        : EntityState.Added;
                }

                await db.SaveChangesAsync();
            }
        }

        private static ShoppingCart[] GetCarts(Dictionary<int, CartItem> cartItems)
        {
            var carts = new ShoppingCart[0];
            return carts;
        }

        public static Dictionary<int, CartItem> CartItems { get; set; } = new Dictionary<int, CartItem>();

    }
}
