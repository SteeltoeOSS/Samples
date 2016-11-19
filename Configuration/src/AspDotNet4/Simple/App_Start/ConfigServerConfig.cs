using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Steeltoe.Extensions.Configuration;
using System;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace Simple4
{

    public class ConfigServerConfig
    {

       
        public static IConfigurationRoot Configuration { get; set; }

        public static void RegisterConfig(string environment)
        {
            var env = new HostingEnvironment(environment);

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
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
            ApplicationName = PlatformServices.Default.Application.ApplicationName;
            ContentRootPath = PlatformServices.Default.Application.ApplicationBasePath;
        }

        public string ApplicationName { get; set; }

        public IFileProvider ContentRootFileProvider { get; set; }

        public string ContentRootPath { get; set; }

        public string EnvironmentName { get; set; }

        public IFileProvider WebRootFileProvider { get; set; }

        public string WebRootPath { get; set; }

        IFileProvider IHostingEnvironment.WebRootFileProvider { get; set; }
    }
}
