using CosmosDb.Data;

namespace CosmosDb.Models;

public sealed class CosmosDbViewModel
{
    public List<SampleObject> SampleObjects { get; set; } = new();
}
