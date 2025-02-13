﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Steeltoe.Samples.MySqlEFCore.Data;
using Steeltoe.Samples.MySqlEFCore.Entities;

namespace Steeltoe.Samples.MySqlEFCore;

internal sealed class MySqlSeeder
{
    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        await using var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await DropCreateTablesAsync(appDbContext);
        await InsertSampleDataAsync(appDbContext);
    }

    public static async Task CreateOtherSampleDataAsync(IServiceProvider serviceProvider)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        await using var otherDbContext = scope.ServiceProvider.GetRequiredService<OtherDbContext>();

        await DropCreateTablesAsync(otherDbContext);
        await InsertOtherSampleDataAsync(otherDbContext);
    }

    private static async Task DropCreateTablesAsync(DbContext dbContext)
    {
        bool wasCreated = await dbContext.Database.EnsureCreatedAsync();

        if (!wasCreated)
        {
            // The database already existed. Because apps usually don't have permission to drop the database,
            // we drop and recreate all the tables in the DbContext instead.
            var databaseCreator = (RelationalDatabaseCreator)dbContext.Database.GetService<IDatabaseCreator>();

            await DropTablesAsync(dbContext);
            await databaseCreator.CreateTablesAsync();
        }
    }

    private static async Task DropTablesAsync(DbContext dbContext)
    {
        IEnumerable<string> tableNames = dbContext.Model.GetEntityTypes().Select(type => type.GetSchemaQualifiedTableName()!);
        IEnumerable<string> dropStatements = tableNames.Select(tableName => "DROP TABLE IF EXISTS `" + tableName + "`;");

        string sqlStatement = string.Join(Environment.NewLine, dropStatements);
        await dbContext.Database.ExecuteSqlRawAsync(sqlStatement);
    }

    private static async Task InsertSampleDataAsync(AppDbContext appDbContext)
    {
        appDbContext.SampleEntities.AddRange(new SampleEntity
        {
            Text = "Test Data 1 - AppDbContext"
        }, new SampleEntity
        {
            Text = "Test Data 2 - AppDbContext"
        });

        await appDbContext.SaveChangesAsync();
    }

    private static async Task InsertOtherSampleDataAsync(OtherDbContext otherDbContext)
    {
        otherDbContext.OtherEntities.AddRange(new OtherEntity
        {
            Text = "Test Data 1 - OtherDbContext"
        }, new OtherEntity
        {
            Text = "Test Data 2 - OtherDbContext"
        });

        await otherDbContext.SaveChangesAsync();
    }
}
