using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.PostgreSqlEFCore.Data;
using Steeltoe.Samples.PostgreSqlEFCore.Models;

namespace Steeltoe.Samples.PostgreSqlEFCore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _appDbContext;

    public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        _appDbContext = appDbContext;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch data from PostgreSQL table.
        return View(new PostgreSqlViewModel
        {
            ConnectionString = _appDbContext.Database.GetConnectionString(),
            SampleEntities = await _appDbContext.SampleEntities.ToListAsync(cancellationToken)
        });
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
