using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MySqlEF6.Models
{
    public class SampleData
    {
        internal static async Task InitializeMyContexts(IServiceProvider serviceProvider)
        {
         
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
    
            Database.SetInitializer<TestContext>(new DropCreateDatabaseAlways<TestContext>());
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
            db.Entry(item).State = EntityState.Added;
        }
    }
}
