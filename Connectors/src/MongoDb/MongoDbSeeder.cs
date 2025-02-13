﻿using MongoDB.Driver;
using Steeltoe.Connectors;
using Steeltoe.Connectors.MongoDb;
using Steeltoe.Samples.MongoDb.Data;

namespace Steeltoe.Samples.MongoDb;

internal sealed class MongoDbSeeder
{
    public static async Task CreateSampleDataAsync(IServiceProvider serviceProvider)
    {
        var connectorFactory = serviceProvider.GetRequiredService<ConnectorFactory<MongoDbOptions, IMongoClient>>();
        Connector<MongoDbOptions, IMongoClient> connector = connectorFactory.Get();
        IMongoClient client = connector.GetConnection();

        IMongoCollection<SampleObject> collection = await DropCreateCollectionAsync(client, connector.Options.Database!);
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
