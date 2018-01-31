using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
                Console.WriteLine("Ensuring database has been created...");
                var db = serviceScope.ServiceProvider.GetService<TestContext>();
                if (!db.Database.EnsureCreated())
                {
                    Console.WriteLine("There may be another table in this database already, attempting to create with a workaround");
                    RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)db.Database.GetService<IDatabaseCreator>();
                    databaseCreator.CreateTables();
                }
            }
            InitializeContext(serviceProvider);
        }

        private static void InitializeContext(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<TestContext>();
                if (db.TestData.Any())
                {
                    return;
                }

                AddData<TestData>(db, new TestData() { Id = 1, Data = "Test Data 1 - EF Core TestContext A" });
                AddData<TestData>(db, new TestData() { Id = 2, Data = "Test Data 2 - EF Core TestContext B" });
                db.SaveChanges();
            }
        }

        private static void AddData<TData>(DbContext db, object item) where TData: class
        {
            db.Entry(item).State = EntityState.Added;
        }
    }
}
