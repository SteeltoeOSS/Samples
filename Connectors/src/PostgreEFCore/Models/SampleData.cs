using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
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
         
                await db.Database.MigrateAsync();
            }

            await InitializeContext(serviceProvider);

        }

        private static async Task InitializeContext(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<TestContext>();
                AddData<TestData>(db, new TestData() { Id = 1, Data = "Test Data 1 - TestContext " });
                AddData<TestData>(db, new TestData() { Id = 2, Data = "Test Data 2 - TestContext " });
                await db.SaveChangesAsync();
            }
        }

        private static void AddData<TData>(DbContext db, object item) where TData: class
        {
            if (db.Entry(item) != null)
            {
                return;
            }

            db.Entry(item).State = EntityState.Added;
        }
    }
}
