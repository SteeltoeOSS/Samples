using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySql.Models;
using Steeltoe.Connector;
using Steeltoe.Connector.MySql;

namespace MySql.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ConnectionFactory<MySqlOptions, MySqlConnection> _connectionFactory;

    public HomeController(ILogger<HomeController> logger, ConnectionFactory<MySqlOptions, MySqlConnection> connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch data from MySQL table.
        var model = new MySqlViewModel
        {
            ConnectionString = _connectionFactory.GetDefaultConnectionString()
        };

        await using MySqlConnection connection = _connectionFactory.GetDefaultConnection();
        await connection.OpenAsync(cancellationToken);
        var command = new MySqlCommand("SELECT * FROM TestData;", connection);
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
