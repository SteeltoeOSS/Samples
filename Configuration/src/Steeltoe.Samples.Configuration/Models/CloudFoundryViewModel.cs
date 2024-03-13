using Steeltoe.Configuration.CloudFoundry;

namespace Steeltoe.Samples.Configuration.Models;

public class CloudFoundryViewModel(CloudFoundryApplicationOptions appOptions, CloudFoundryServicesOptions servOptions)
{
    public CloudFoundryApplicationOptions Application { get; } = appOptions;

    public CloudFoundryServicesOptions Services { get; } = servOptions;
}
