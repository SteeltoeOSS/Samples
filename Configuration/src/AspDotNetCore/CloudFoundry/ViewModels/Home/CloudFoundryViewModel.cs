using Steeltoe.Extensions.Configuration.CloudFoundry;


namespace CloudFoundry.ViewModels.Home
{
    public class CloudFoundryViewModel
    {
        public CloudFoundryViewModel(CloudFoundryApplicationOptions appOptions, CloudFoundryServicesOptions servOptions)
        {
            CloudFoundryServices = servOptions;
            CloudFoundryApplication = appOptions;
        }
        public CloudFoundryServicesOptions CloudFoundryServices { get;}
        public CloudFoundryApplicationOptions CloudFoundryApplication { get;}
    }
}
