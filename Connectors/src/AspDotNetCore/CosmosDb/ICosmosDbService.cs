using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDb
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<TestData>> GetTestDataAsync(string query);

        Task AddTestDataAsync(TestData item);

        Task DeleteTestDataAsync(string id);
    }
}
