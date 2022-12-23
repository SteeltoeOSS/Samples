using MongoDB.Bson;

namespace MongoDb.Data;

public sealed class SampleObject
{
    public ObjectId Id { get; set; }
    public string? Text { get; set; }
}
