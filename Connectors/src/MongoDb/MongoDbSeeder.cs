using MongoDb.Data;
using MongoDB.Driver;
using Steeltoe.Connector;
using Steeltoe.Connector.MongoDb;

namespace MongoDb;

internal sealed class MongoDbSeeder
{
    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        var connectionFactory = serviceProvider.GetRequiredService<ConnectionFactory<MongoDbOptions, IMongoClient>>();
        ConnectionProvider<MongoDbOptions, IMongoClient> connectionProvider = connectionFactory.GetDefault();
        IMongoClient client = connectionProvider.CreateConnection();

        IMongoCollection<SampleObject> collection = await DropCreateCollectionAsync(client, connectionProvider.Options.Database);
        await InsertSampleDataAsync(collection);
    }

    private static async Task<IMongoCollection<SampleObject>> DropCreateCollectionAsync(IMongoClient client, string databaseName)
    {
        IMongoDatabase database = client.GetDatabase(databaseName);

        await database.DropCollectionAsync("SampleObjects");
        return database.GetCollection<SampleObject>("SampleObjects");
    }

    private static async Task InsertSampleDataAsync(IMongoCollection<SampleObject> collection)
    {
        await collection.InsertManyAsync(new[]
        {
            new SampleObject
            {
                Text = "Object1"
            },
            new SampleObject
            {
                Text = "Object2"
            }
        });
    }
}
