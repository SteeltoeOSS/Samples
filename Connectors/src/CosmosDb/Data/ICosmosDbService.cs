namespace CosmosDb.Data;

public interface ICosmosDbService
{
    IAsyncEnumerable<SampleObject> GetAllAsync(CancellationToken cancellationToken);
}
