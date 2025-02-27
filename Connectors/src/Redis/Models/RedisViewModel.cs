namespace Steeltoe.Samples.Redis.Models;

public sealed class RedisViewModel
{
    public string? ConnectionString { get; set; }

    public IDictionary<string, string?> DistributedCacheData { get; set; } = new Dictionary<string, string?>();
    public IDictionary<string, string?> ConnectionMultiplexerData { get; set; } = new Dictionary<string, string?>();
    public string? LuaResult { get; set; }
}
