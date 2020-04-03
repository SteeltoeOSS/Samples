using Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDb
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly CosmosContainer _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            _container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddTestDataAsync(TestData item)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task DeleteTestDataAsync(string id)
        {
            await _container.DeleteItemAsync<TestData>(id, new PartitionKey(id));
        }

        public async Task<IEnumerable<TestData>> GetTestDataAsync(string queryString)
        {
            var results = new List<TestData>();
            await foreach (var t in _container.GetItemQueryIterator<TestData>(new QueryDefinition(queryString)))
            {
                results.Add(t);
            }

            return results;
        }
    }
}
