namespace Steeltoe.Samples.ConfigurationProviders.Models;

public sealed class PlaceholderValues
{
    public string? ResolvedFromPathEnvironmentVariable { get; set; }
    public string? Unresolved { get; set; }
    public string? ResolvedFromJson { get; set; }
}
