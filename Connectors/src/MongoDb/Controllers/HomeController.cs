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
    private readonly ConnectionProvider<MongoDbOptions, IMongoClient> _connectionProvider;

    public HomeController(ILogger<HomeController> logger, ConnectionFactory<MongoDbOptions, IMongoClient> connectionFactory)
    {
        _logger = logger;
        _connectionProvider = connectionFactory.GetDefault();
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch objects from MongoDB collection.
        var model = new MongoDbViewModel
        {
            ConnectionString = _connectionProvider.Options.ConnectionString,
            Database = _connectionProvider.Options.Database
        };

        IMongoClient client = _connectionProvider.GetConnection();
        IMongoDatabase database = client.GetDatabase(_connectionProvider.Options.Database);
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
