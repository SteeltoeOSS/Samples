using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Steeltoe.Connectors;
using Steeltoe.Connectors.PostgreSql;
using Steeltoe.Samples.PostgreSql.Models;

namespace Steeltoe.Samples.PostgreSql.Controllers;

public sealed class HomeController(ConnectorFactory<PostgreSqlOptions, NpgsqlConnection> connectorFactory) : Controller
{
    private readonly Connector<PostgreSqlOptions, NpgsqlConnection> _connector = connectorFactory.Get();

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch data from PostgreSQL table.
        var model = new PostgreSqlViewModel
        {
            ConnectionString = _connector.Options.ConnectionString
        };

        await using NpgsqlConnection connection = _connector.GetConnection();
        await connection.OpenAsync(cancellationToken);
        var command = new NpgsqlCommand("SELECT * FROM TestData;", connection);
        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            string idValue = reader[0].ToString()!;
            string? textValue = reader[1].ToString();

            model.Rows.Add(idValue, textValue);
        }

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
