﻿namespace Steeltoe.Samples.ConfigurationProviders.Models;

public sealed class PlaceholderValues
{
    public string? ResolvedFromEnvironmentVariables { get; set; }
    public string? Unresolved { get; set; }
    public string? ResolvedFromJson { get; set; }
}
