namespace Steeltoe.Samples.Redis.Models;

public sealed class RedisViewModel
{
    public string? ConnectionString { get; set; }

    public Dictionary<string, string?> DistributedCacheData { get; set; } = new();
    public Dictionary<string, string?> ConnectionMultiplexerData { get; set; } = new();
    public string? LuaResult { get; set; }
}
