using System.Runtime.CompilerServices;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CosmosDb.Data;

internal sealed class CosmosDbService : ICosmosDbService
{
    private readonly CosmosClient _cosmosClient;
    private readonly string _databaseId;
    private readonly string _containerId;

    private Container Container => _cosmosClient.GetContainer(_databaseId, _containerId);

    public CosmosDbService(CosmosClient cosmosClient, string databaseId, string containerId)
    {
        _cosmosClient = cosmosClient;
        _databaseId = databaseId;
        _containerId = containerId;
    }

    public async IAsyncEnumerable<SampleObject> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using FeedIterator<SampleObject> iterator = Container.GetItemLinqQueryable<SampleObject>().ToFeedIterator();

        while (iterator.HasMoreResults)
        {
            FeedResponse<SampleObject> response = await iterator.ReadNextAsync(cancellationToken);

            foreach (SampleObject sampleObject in response)
            {
                yield return sampleObject;
            }
        }
    }
}
