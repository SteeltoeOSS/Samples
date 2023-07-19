using System.Net;
using CosmosDb.Data;
using Microsoft.Azure.Cosmos;
using Steeltoe.Connectors;
using Steeltoe.Connectors.CosmosDb;

namespace CosmosDb;

internal sealed class CosmosDbSeeder
{
    public const string ContainerId = "TestContainer";

    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        var connectorFactory = serviceProvider.GetRequiredService<ConnectorFactory<CosmosDbOptions, CosmosClient>>();
        Connector<CosmosDbOptions, CosmosClient> connector = connectorFactory.Get();

        // Do not dispose the CosmosClient singleton.
        CosmosClient client = connector.GetConnection();
        Container container = await DropCreateDatabaseAsync(client, connector.Options.Database!);

        await InsertSampleDataAsync(container);
    }

    private static async Task<Container> DropCreateDatabaseAsync(CosmosClient cosmosClient, string databaseId)
    {
        DatabaseResponse response = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            await response.Database.DeleteAsync();
            response = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }

        return await response.Database.CreateContainerIfNotExistsAsync(ContainerId, "/id");
    }

    private static async Task InsertSampleDataAsync(Container container)
    {
        var sampleObject1 = new SampleObject(Guid.NewGuid().ToString())
        {
            Text = "Object1"
        };

        await container.CreateItemAsync(sampleObject1, new PartitionKey(sampleObject1.Id));

        var sampleObject2 = new SampleObject(Guid.NewGuid().ToString())
        {
            Text = "Object2"
        };

        await container.CreateItemAsync(sampleObject2, new PartitionKey(sampleObject2.Id));
    }
}
