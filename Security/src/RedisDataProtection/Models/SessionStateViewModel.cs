namespace Steeltoe.Samples.RedisDataProtection.Models;

public sealed class SessionStateViewModel
{
    public string? InstanceIndex { get; set; }
    public string? SessionId { get; set; }
    public string? SessionValue { get; set; }
}
