using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PostgreSql.Models;
using Steeltoe.Connector;
using Steeltoe.Connector.PostgreSql;

namespace PostgreSql.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ConnectionFactory<PostgreSqlOptions, NpgsqlConnection> _connectionFactory;

    public HomeController(ILogger<HomeController> logger, ConnectionFactory<PostgreSqlOptions, NpgsqlConnection> connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch data from PostgreSQL table.
        var model = new PostgreSqlViewModel
        {
            ConnectionString = _connectionFactory.GetDefaultConnectionString()
        };

        await using NpgsqlConnection connection = _connectionFactory.GetDefaultConnection();
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
