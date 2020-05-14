using System.Text.Json;
using System.Text.Json.Serialization;

namespace CosmosDb
{
    public class TestData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public string SomeThing { get; set; }

        public string SomeOtherThing { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
