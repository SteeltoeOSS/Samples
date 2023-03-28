using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Data;
using MongoDB.Driver;
using MongoDb.Models;
using Steeltoe.Connector;
using Steeltoe.Connector.MongoDb;

namespace MongoDb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ConnectionFactory<MongoDbOptions, MongoClient> _connectionFactory;

    public HomeController(ILogger<HomeController> logger, ConnectionFactory<MongoDbOptions, MongoClient> connectionFactory)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch objects from MongoDB collection.
        var model = new MongoDbViewModel
        {
            ConnectionString = _connectionFactory.GetDefaultConnectionString()
        };

        IMongoClient client = _connectionFactory.GetDefaultConnection();
        IMongoDatabase database = client.GetDatabase("TestDatabase");
        IMongoCollection<SampleObject> collection = database.GetCollection<SampleObject>("SampleObjects");
        model.SampleObjects = await collection.Find(obj => true).ToListAsync(cancellationToken);

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
