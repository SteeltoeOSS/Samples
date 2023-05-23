using Npgsql;
using Steeltoe.Connectors;
using Steeltoe.Connectors.PostgreSql;

namespace PostgreSql;

internal sealed class PostgreSqlSeeder
{
    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        var connectorFactory = serviceProvider.GetRequiredService<ConnectorFactory<PostgreSqlOptions, NpgsqlConnection>>();
        await using NpgsqlConnection connection = connectorFactory.GetDefault().GetConnection();

        await connection.OpenAsync();

        await DropCreateTableAsync(connection);
        await InsertSampleDataAsync(connection);
    }

    private static async Task DropCreateTableAsync(NpgsqlConnection connection)
    {
        var dropCommand = new NpgsqlCommand("DROP TABLE IF EXISTS TestData;", connection);
        await dropCommand.ExecuteNonQueryAsync();

        var createCommand = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS TestData(Id INT PRIMARY KEY, MyText VARCHAR(255));", connection);
        await createCommand.ExecuteNonQueryAsync();
    }

    private static async Task InsertSampleDataAsync(NpgsqlConnection connection)
    {
        var insertCommand1 = new NpgsqlCommand("INSERT INTO TestData(Id, MyText) VALUES(1, 'Row1 Text');", connection);
        await insertCommand1.ExecuteNonQueryAsync();

        var insertCommand2 = new NpgsqlCommand("INSERT INTO TestData(Id, MyText) VALUES(2, 'Row2 Text');", connection);
        await insertCommand2.ExecuteNonQueryAsync();
    }
}
