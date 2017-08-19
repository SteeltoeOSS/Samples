using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


using Microsoft.EntityFrameworkCore;

namespace ShoppingCartService.Models
{
    public static class SampleData
    {
        public static void InitializeShoppingCartDatabase(IServiceProvider serviceProvider)
        {
            if (ShouldDropCreateDatabase())
            {
                using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var db = serviceScope.ServiceProvider.GetService<ShoppingCartContext>();

                    db.Database.EnsureCreated();

                    InsertTestData(serviceProvider);
                }
            }
        }

        private static void InsertTestData(IServiceProvider serviceProvider)
        {
            var carts = GetCarts(CartItems);
            AddOrUpdate(serviceProvider, c => c.CartId, carts);
        }

        // TODO [EF] This may be replaced by a first class mechanism in EF
        private static void AddOrUpdate<TEntity>(
            IServiceProvider serviceProvider,
            Func<TEntity, object> propertyToMatch, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            // Query in a separate context so that we can attach existing entities as modified
            List<TEntity> existingData;
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ShoppingCartContext>();
                existingData = db.Set<TEntity>().ToList();
            }

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ShoppingCartContext>();
                foreach (var item in entities)
                {
                    var exists = existingData.Any(g => propertyToMatch(g).Equals(propertyToMatch(item)));
                    if (!exists)
                        db.Entry(item).State = EntityState.Added;
                }

                db.SaveChanges();
            }
        }

        private static ShoppingCart[] GetCarts(Dictionary<int, CartItem> cartItems)
        {
            var carts = new ShoppingCart[0];
            return carts;
        }

        public static Dictionary<int, CartItem> CartItems { get; set; } = new Dictionary<int, CartItem>();
        private static bool ShouldDropCreateDatabase()
        {
            string index = Environment.GetEnvironmentVariable("CF_INSTANCE_INDEX");
            if (string.IsNullOrEmpty(index))
            {
                return true;
            }
            int indx = -1;
            if (int.TryParse(index, out indx))
            {
                if (indx > 0) return false;
            }
            return true;
        }
    }
}
