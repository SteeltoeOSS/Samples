﻿using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MySqlEFCore.Data;
using MySqlEFCore.Entities;

namespace MySqlEFCore;

internal sealed class MySqlSeeder
{
    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();

        try
        {
            await using var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await DropCreateTablesAsync(appDbContext);
            await InsertSampleDataAsync(appDbContext);
        }
        catch (DbException exception)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<MySqlSeeder>>();
            logger.LogError(exception, "An error occurred seeding the DB.");
        }
    }

    public static async Task CreateOtherSampleDataAsync(IServiceProvider serviceProvider)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();

        try
        {
            await using var otherDbContext = scope.ServiceProvider.GetRequiredService<OtherDbContext>();

            await DropCreateTablesAsync(otherDbContext);
            await InsertOtherSampleDataAsync(otherDbContext);
        }
        catch (DbException exception)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<MySqlSeeder>>();
            logger.LogError(exception, "An error occurred seeding the DB.");
        }
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
