namespace Steeltoe.Samples.Configuration.Models;

public class PlaceholderValues
{
      public string? ResolvedPlaceholderFromEnvVariables { get; set; }

      public string? UnresolvedPlaceholder { get; set; }

      public string? ResolvedPlaceholderFromJson { get; set; }
}
