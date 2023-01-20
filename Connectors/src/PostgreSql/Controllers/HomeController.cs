using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PostgreSql.Models;

namespace PostgreSql.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly NpgsqlConnection _npgsqlConnection;

    public HomeController(ILogger<HomeController> logger, NpgsqlConnection npgsqlConnection)
    {
        _logger = logger;
        _npgsqlConnection = npgsqlConnection;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch data from PostgreSQL table.
        var model = new PostgreSqlViewModel();

        await _npgsqlConnection.OpenAsync(cancellationToken);
        var command = new NpgsqlCommand("SELECT * FROM TestData;", _npgsqlConnection);
        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            string idValue = reader[0].ToString()!;
            string? textValue = reader[1].ToString();

            model.Rows.Add(idValue, textValue);
        }

        model.DatabaseName = _npgsqlConnection.Database;

#if DEBUG
        model.ServerName = _npgsqlConnection.DataSource;
#endif

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
