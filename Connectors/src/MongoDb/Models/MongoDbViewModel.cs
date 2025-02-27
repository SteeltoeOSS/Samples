using Steeltoe.Samples.MongoDb.Data;

namespace Steeltoe.Samples.MongoDb.Models;

public sealed class MongoDbViewModel
{
    public string? ConnectionString { get; set; }
    public string? Database { get; set; }

    public List<SampleObject> SampleObjects { get; set; } = [];
}
