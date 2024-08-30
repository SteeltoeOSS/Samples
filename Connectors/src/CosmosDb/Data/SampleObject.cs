using Newtonsoft.Json;

namespace Steeltoe.Samples.CosmosDb.Data;

public sealed class SampleObject(string id)
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; } = id;

    public string? Text { get; set; }
}
