
using Microsoft.AspNet.FileProviders;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using SteelToe.Extensions.Configuration.CloudFoundry;
using Pivotal.Extensions.Configuration;


namespace SimpleCloudFoundry4
{
    public class ServerConfig
    {
        public static CloudFoundryApplicationOptions CloudFoundryApplication
        {
            get
            {
                return ConfigurationBinder.Get<CloudFoundryApplicationOptions>(Configuration, new CloudFoundryApplicationOptions());
            }
        }
        public static CloudFoundryServicesOptions CloudFoundryServices
        {
            get
            {
                return ConfigurationBinder.Get<CloudFoundryServicesOptions>(Configuration, new CloudFoundryServicesOptions());
            }
        }

        public static IConfigurationRoot Configuration { get; set; }

        public static void RegisterConfig(string environment)
        {
            var env = new HostingEnvironment(environment);

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()

            // Adds the Spring Cloud Configuration Server as a configuration source.
            // The settings used in contacting the Server will be picked up from
            // appsettings.json, and then overriden from any environment variables, and then
            // overriden from the CloudFoundry environment variable settings. 
            // Defaults will be used for any settings not present in any of the earlier added 
            // sources.  See ConfigServerClientSettings for defaults.
            .AddConfigServer(env);

            Configuration = builder.Build();

        }
    }
    public class HostingEnvironment : IHostingEnvironment
    {
        public HostingEnvironment(string env)
        {
            EnvironmentName = env;
        }
        public string EnvironmentName { get; set; }

        public IFileProvider WebRootFileProvider { get; set; }

        public string WebRootPath { get; set; }

    }
}