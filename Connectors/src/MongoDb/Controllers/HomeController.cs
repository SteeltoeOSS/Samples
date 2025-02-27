using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Steeltoe.Connectors;
using Steeltoe.Connectors.MongoDb;
using Steeltoe.Samples.MongoDb.Data;
using Steeltoe.Samples.MongoDb.Models;

namespace Steeltoe.Samples.MongoDb.Controllers;

public sealed class HomeController(ConnectorFactory<MongoDbOptions, IMongoClient> connectorFactory) : Controller
{
    private readonly Connector<MongoDbOptions, IMongoClient> _connector = connectorFactory.Get();

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch objects from MongoDB collection.
        var model = new MongoDbViewModel
        {
            ConnectionString = _connector.Options.ConnectionString,
            Database = _connector.Options.Database
        };

        IMongoClient client = _connector.GetConnection();
        IMongoDatabase database = client.GetDatabase(_connector.Options.Database);
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
