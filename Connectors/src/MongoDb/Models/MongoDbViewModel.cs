using MongoDb.Data;

namespace MongoDb.Models;

public sealed class MongoDbViewModel
{
    public string? ConnectionString { get; set; }

    public List<SampleObject> SampleObjects { get; set; } = new();
}
