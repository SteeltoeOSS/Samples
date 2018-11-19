using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MySqlEFCore
{
    public class MoreSampleData
    {
        internal static void InitializeMyContexts(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                Console.WriteLine("Ensuring second database has been created...");
                var db = serviceScope.ServiceProvider.GetService<SecondTestContext>();
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
                var db = serviceScope.ServiceProvider.GetService<SecondTestContext>();
                if (db.MoreTestData.Any())
                {
                    return;
                }

                AddData<MoreTestData>(db, new MoreTestData() { Id = 1, Data = "More Test Data 1 - EF Core TestContext A" });
                AddData<MoreTestData>(db, new MoreTestData() { Id = 2, Data = "More Test Data 2 - EF Core TestContext B" });
                db.SaveChanges();
            }
        }

        private static void AddData<TData>(DbContext db, object item) where TData: class
        {
            db.Entry(item).State = EntityState.Added;
        }
    }
}
