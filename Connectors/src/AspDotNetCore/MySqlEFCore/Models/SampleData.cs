using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MySqlEFCore
{
    public class SampleData
    {
        internal static void InitializeMyContexts(IServiceProvider serviceProvider)
        {
         
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<TestContext>();
                db.Database.EnsureCreated();
               
            }
            InitializeContext(serviceProvider);
        }

        private static void InitializeContext(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<TestContext>();
                if (DataExists<TestData>(db))
                    return;

                AddData<TestData>(db, new TestData() { Id = 1, Data = "Test Data 1 - TestContext " });
                AddData<TestData>(db, new TestData() { Id = 2, Data = "Test Data 2 - TestContext " });
                db.SaveChanges();
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
