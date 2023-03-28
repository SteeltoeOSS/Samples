using MongoDb.Data;
using MongoDB.Driver;
using Steeltoe.Connector;
using Steeltoe.Connector.MongoDb;

namespace MongoDb;

internal sealed class MongoDbSeeder
{
    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        var connectionFactory = serviceProvider.GetRequiredService<ConnectionFactory<MongoDbOptions, MongoClient>>();
        IMongoClient client = connectionFactory.GetDefaultConnection();

        IMongoCollection<SampleObject> collection = await DropCreateCollectionAsync(client);
        await InsertSampleDataAsync(collection);
    }

    private static async Task<IMongoCollection<SampleObject>> DropCreateCollectionAsync(IMongoClient client)
    {
        IMongoDatabase database = client.GetDatabase("TestDatabase");

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
