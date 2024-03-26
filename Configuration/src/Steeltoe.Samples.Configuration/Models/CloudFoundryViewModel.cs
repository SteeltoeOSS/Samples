using Steeltoe.Configuration.CloudFoundry;

namespace Steeltoe.Samples.Configuration.Models;

public class CloudFoundryViewModel(CloudFoundryApplicationOptions applicationOptions, CloudFoundryServicesOptions servicesOptions)
{
    public CloudFoundryApplicationOptions Application { get; } = applicationOptions;

    public CloudFoundryServicesOptions Services { get; } = servicesOptions;
}
