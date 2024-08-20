using Steeltoe.Samples.CosmosDb.Data;

namespace Steeltoe.Samples.CosmosDb.Models;

public sealed class CosmosDbViewModel
{
    public string? ConnectionString { get; set; }
    public string? OptionsJson { get; set; }
    public string? Database { get; set; }

    public List<SampleObject> SampleObjects { get; set; } = [];
}
