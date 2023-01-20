using System.Data.Common;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySql.Models;

namespace MySql.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MySqlConnection _mySqlConnection;

    public HomeController(ILogger<HomeController> logger, MySqlConnection mySqlConnection)
    {
        _logger = logger;
        _mySqlConnection = mySqlConnection;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch data from MySQL table.
        var model = new MySqlViewModel();

        await _mySqlConnection.OpenAsync(cancellationToken);
        var command = new MySqlCommand("SELECT * FROM TestData;", _mySqlConnection);
        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            string idValue = reader[0].ToString()!;
            string? textValue = reader[1].ToString();

            model.Rows.Add(idValue, textValue);
        }

        model.DatabaseName = _mySqlConnection.Database;

#if DEBUG
        model.ServerName = _mySqlConnection.DataSource;
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
