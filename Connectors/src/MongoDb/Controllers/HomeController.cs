using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Data;
using MongoDB.Driver;
using MongoDb.Models;

namespace MongoDb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMongoClient _mongoClient;

    public HomeController(ILogger<HomeController> logger, IMongoClient mongoClient)
    {
        _logger = logger;
        _mongoClient = mongoClient;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch objects from MongoDB collection.
        IMongoDatabase database = _mongoClient.GetDatabase("TestDatabase");
        IMongoCollection<SampleObject> collection = database.GetCollection<SampleObject>("SampleObjects");
        List<SampleObject> objects = await collection.Find(obj => true).ToListAsync(cancellationToken);

        return View(new MongoDbViewModel
        {
            SampleObjects = objects
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
