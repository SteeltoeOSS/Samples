using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Steeltoe.Connectors;
using Steeltoe.Connectors.CosmosDb;
using Steeltoe.Samples.CosmosDb.Data;
using Steeltoe.Samples.CosmosDb.Models;

namespace Steeltoe.Samples.CosmosDb.Controllers;

public sealed class HomeController(ConnectorFactory<CosmosDbOptions, CosmosClient> connectorFactory) : Controller
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private readonly Connector<CosmosDbOptions, CosmosClient> _connector = connectorFactory.Get();

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Fetch objects from CosmosDB container. Do not dispose the CosmosClient singleton.
        CosmosClient client = _connector.GetConnection();
        Container container = client.GetContainer(_connector.Options.Database, CosmosDbSeeder.ContainerId);

        var model = new CosmosDbViewModel
        {
            ConnectionString = _connector.Options.ConnectionString,
            OptionsJson = JsonSerializer.Serialize(client.ClientOptions, SerializerOptions),
            Database = _connector.Options.Database
        };

        await foreach (SampleObject sampleObject in GetAllAsync(container, cancellationToken))
        {
            model.SampleObjects.Add(sampleObject);
        }

        return View(model);
    }

    private static async IAsyncEnumerable<SampleObject> GetAllAsync(Container container, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using FeedIterator<SampleObject> iterator = container.GetItemLinqQueryable<SampleObject>().ToFeedIterator();

        while (iterator.HasMoreResults)
        {
            FeedResponse<SampleObject> response = await iterator.ReadNextAsync(cancellationToken);

            foreach (SampleObject sampleObject in response)
            {
                yield return sampleObject;
            }
        }
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
