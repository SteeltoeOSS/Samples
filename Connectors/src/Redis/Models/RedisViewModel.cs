namespace Redis.Models;

public sealed class RedisViewModel
{
    public Dictionary<string, string?> CacheData { get; set; } = new();
    public Dictionary<string, string?> MultiplexerData { get; set; } = new();
    public string? LuaResult { get; set; }
}
