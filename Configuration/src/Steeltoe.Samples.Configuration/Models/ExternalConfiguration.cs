namespace Steeltoe.Samples.Configuration.Models;

/// <summary>
/// This object contains the configuration values provided by a configuration service.
/// The source of these values is here: https://github.com/spring-cloud-samples/config-repo
/// </summary>
public class ExternalConfiguration
{
    public string Bar { get; set; } = "Not Set";

    public string Foo { get; set; } = "Not Set";

    public ExternalConfigurationInfo ExternalConfigurationInfo { get; set; } = new();
}