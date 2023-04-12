using System.Diagnostics;
using CosmosDb.Data;
using CosmosDb.Models;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICosmosDbService _cosmosDbService;

    public HomeController(ILogger<HomeController> logger, ICosmosDbService cosmosDbService)
    {
        _logger = logger;
        _cosmosDbService = cosmosDbService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch objects from CosmosDB container.
        var model = new CosmosDbViewModel();

        await foreach (SampleObject sampleObject in _cosmosDbService.GetAllAsync(cancellationToken))
        {
            model.SampleObjects.Add(sampleObject);
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
