using System.Net;
using CosmosDb.Data;
using Microsoft.Azure.Cosmos;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Connector;
using Steeltoe.Connector.CosmosDb;

namespace CosmosDb;

internal sealed class CosmosDbSeeder
{
    private const string ContainerId = "TestContainer";

    public static async Task<ICosmosDbService> CreateSampleDataAsync(IConfiguration configuration)
    {
        // Read settings from "CosmosDb:Client" and VCAP:services or services.
        var connectionStringManager = new ConnectionStringManager(configuration);
        Connection connection = connectionStringManager.Get<CosmosDbConnectionInfo>();

        string databaseId = connection.Properties["DatabaseId"];
        string connectionString = connection.ConnectionString;
        string applicationName = GetApplicationName(configuration);

        var cosmosClientOptions = new CosmosClientOptions
        {
            ApplicationName = applicationName
        };

        try
        {
            var cosmosClient = new CosmosClient(connectionString, cosmosClientOptions);

            Container container = await DropCreateDatabaseAsync(cosmosClient, databaseId);
            await InsertSampleDataAsync(container);

            return new CosmosDbService(cosmosClient, databaseId, ContainerId);
        }
        catch (CosmosException exception)
        {
            throw new Exception("An error occurred seeding the DB.", exception);
        }
    }

    private static string GetApplicationName(IConfiguration configuration)
    {
        var options = new CloudFoundryApplicationOptions(configuration);
        return options.ApplicationName ?? options.DefaultAppName;
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
