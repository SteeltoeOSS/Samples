using Steeltoe.Configuration.CloudFoundry;

namespace Steeltoe.Samples.ConfigurationProviders.Models;

public sealed class CloudFoundryViewModel(CloudFoundryApplicationOptions applicationOptions, CloudFoundryServicesOptions servicesOptions)
{
    public CloudFoundryApplicationOptions Application { get; } = applicationOptions;
    public CloudFoundryServicesOptions Services { get; } = servicesOptions;
}
