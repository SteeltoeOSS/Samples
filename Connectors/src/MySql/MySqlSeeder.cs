using System.Data.Common;
using MySql.Data.MySqlClient;

namespace MySql;

internal sealed class MySqlSeeder
{
    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();

        try
        {
            await using var connection = scope.ServiceProvider.GetRequiredService<MySqlConnection>();
            await connection.OpenAsync();

            await DropCreateTableAsync(connection);
            await InsertSampleDataAsync(connection);
        }
        catch (DbException exception)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<MySqlSeeder>>();
            logger.LogError(exception, "An error occurred seeding the DB.");
        }
    }

    private static async Task DropCreateTableAsync(MySqlConnection connection)
    {
        var dropCommand = new MySqlCommand("DROP TABLE IF EXISTS TestData;", connection);
        await dropCommand.ExecuteNonQueryAsync();

        var createCommand = new MySqlCommand("CREATE TABLE IF NOT EXISTS TestData(Id INT PRIMARY KEY, MyText VARCHAR(255));", connection);
        await createCommand.ExecuteNonQueryAsync();
    }

    private static async Task InsertSampleDataAsync(MySqlConnection connection)
    {
        var insertCommand1 = new MySqlCommand("INSERT INTO TestData(Id, MyText) VALUES(1, 'Row1 Text');", connection);
        await insertCommand1.ExecuteNonQueryAsync();

        var insertCommand2 = new MySqlCommand("INSERT INTO TestData(Id, MyText) VALUES(2, 'Row2 Text');", connection);
        await insertCommand2.ExecuteNonQueryAsync();
    }
}
