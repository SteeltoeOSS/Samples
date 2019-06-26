using Microsoft.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.IO;

namespace SMBFileShares4x
{
    public class ApplicationConfig
    {
        public static CloudFoundryApplicationOptions CloudFoundryApplication
        {

            get
            {
                var opts = new CloudFoundryApplicationOptions();
                var appSection = Configuration.GetSection(CloudFoundryApplicationOptions.CONFIGURATION_PREFIX);
                appSection.Bind(opts);
                return opts;
            }
        }
        public static CloudFoundryServicesOptions CloudFoundryServices
        {
            get
            {
                var opts = new CloudFoundryServicesOptions();
                var serviceSection = Configuration.GetSection(CloudFoundryServicesOptions.CONFIGURATION_PREFIX);
                serviceSection.Bind(opts);
                return opts;
            }
        }
        public static IConfigurationRoot Configuration { get; set; }

        public static void Configure(string environment)
        {

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetContentRoot())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCloudFoundry();

            Configuration = builder.Build();
        }

        public static string GetContentRoot()
        {
            var basePath = (string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") ??
               AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }
    }
}
