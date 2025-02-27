using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.SqlServerEFCore.Data;
using Steeltoe.Samples.SqlServerEFCore.Models;

namespace Steeltoe.Samples.SqlServerEFCore.Controllers;

public sealed class HomeController(AppDbContext appDbContext) : Controller
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch data from SQL Server table.
        return View(new SqlServerViewModel
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
