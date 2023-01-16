using Newtonsoft.Json;

namespace CosmosDb.Data;

public sealed class SampleObject
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    public string? Text { get; set; }

    public SampleObject(string id)
    {
        Id = id;
    }
}
