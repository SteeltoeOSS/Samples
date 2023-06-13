﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Data;
using MongoDB.Driver;
using MongoDb.Models;
using Steeltoe.Connectors;
using Steeltoe.Connectors.MongoDb;

namespace MongoDb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Connector<MongoDbOptions, IMongoClient> _connector;

    public HomeController(ILogger<HomeController> logger, ConnectorFactory<MongoDbOptions, IMongoClient> connectorFactory)
    {
        _logger = logger;
        _connector = connectorFactory.Get();
    }

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
