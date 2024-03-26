namespace Steeltoe.Samples.Configuration.Models;

public class PlaceholderValues
{
      public string? ResolvedFromEnvironmentVariables { get; set; }

      public string? Unresolved { get; set; }

      public string? ResolvedFromJson { get; set; }
}
