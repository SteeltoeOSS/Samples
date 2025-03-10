using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.MySqlEFCore.Data;
using Steeltoe.Samples.MySqlEFCore.Models;

namespace Steeltoe.Samples.MySqlEFCore.Controllers;

public sealed class HomeController(IServiceProvider serviceProvider) : Controller
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch data from MySQL table.
        await using var appDbContext = _serviceProvider.GetRequiredService<AppDbContext>();

        var model = new MySqlViewModel
        {
            ConnectionString = appDbContext.Database.GetConnectionString(),
            SampleEntities = await appDbContext.SampleEntities.ToListAsync(cancellationToken)
        };

        await using var otherDbContext = _serviceProvider.GetService<OtherDbContext>();

        if (otherDbContext != null)
        {
            model.OtherConnectionString = otherDbContext.Database.GetConnectionString();
            model.OtherEntities = await otherDbContext.OtherEntities.ToListAsync(cancellationToken);
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
