namespace Steeltoe.Samples.AuthConsole;

internal sealed class CloudFoundryConventions
{
    public const string ConfigurationPrefix = "CloudFoundryConventions";

    public string ApiUriSegment { get; set; } = string.Empty;

    public string AppsUriSegment { get; set; } = string.Empty;
}
