using MongoDb.Data;
using MongoDB.Driver;

namespace MongoDb;

internal sealed class MongoDbSeeder
{
    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        try
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();

            IMongoCollection<SampleObject> collection = await DropCreateCollectionAsync(client);
            await InsertSampleDataAsync(collection);
        }
        catch (Exception exception)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<MongoDbSeeder>>();
            logger.LogError(exception, "An error occurred seeding the DB.");
        }
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
