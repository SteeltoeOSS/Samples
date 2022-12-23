using MongoDb.Data;

namespace MongoDb.Models;

public sealed class MongoDbViewModel
{
    public List<SampleObject> SampleObjects { get; set; } = new();
}
