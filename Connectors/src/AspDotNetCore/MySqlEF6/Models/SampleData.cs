using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Entity;

namespace MySqlEF6.Models
{
    public class SampleData
    {
        internal static void InitializeMyContexts(IServiceProvider serviceProvider)
        {
         
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
    
            Database.SetInitializer<TestContext>(new DropCreateDatabaseAlways<TestContext>());
            InitializeContext(serviceProvider);
        }

        private static void InitializeContext(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<TestContext>();
                AddData<TestData>(db, new TestData() { Id = 1, Data = "Test Data 1 - TestContext " });
                AddData<TestData>(db, new TestData() { Id = 2, Data = "Test Data 2 - TestContext " });
                db.SaveChanges();
            }
        }

        private static void AddData<TData>(DbContext db, object item) where TData: class
        {
            db.Entry(item).State = EntityState.Added;
        }
    }
}
