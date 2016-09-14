using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PostgreEFCore
{
    public class SampleData
    {
        internal static async Task InitializeMyContexts(IServiceProvider serviceProvider)
        {
         
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<TestContext>();
                await db.Database.EnsureCreatedAsync();
               
            }
            await InitializeContext(serviceProvider);
        }

        private static async Task InitializeContext(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<TestContext>();
                if (DataExists<TestData>(db))
                    return;

                AddData<TestData>(db, new TestData() { Id = 1, Data = "Test Data 1 - TestContext " });
                AddData<TestData>(db, new TestData() { Id = 2, Data = "Test Data 2 - TestContext " });
                await db.SaveChangesAsync();
            }
        }

        private static bool DataExists<TData>(DbContext db) where TData: class
        {
            var existingData = db.Set<TData>().ToList();
            if (existingData.Count > 0)
                return true;
            return false;
        }

        private static void AddData<TData>(DbContext db, object item) where TData: class
        {
            db.Entry(item).State = EntityState.Added;
        }
    }
}
