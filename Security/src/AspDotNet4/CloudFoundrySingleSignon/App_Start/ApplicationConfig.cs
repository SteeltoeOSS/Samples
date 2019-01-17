using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.IO;
using System.Linq;

namespace CloudFoundrySingleSignon.App_Start
{
    public class ApplicationConfig
    {
        public static IConfiguration Configuration { get; private set; }
        public static LoggerFactory LoggerFactory { get; private set; }
        // public static SsoServiceInfo SsoServiceInfo;

        public static void RegisterConfig(string environment)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetContentRoot())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCloudFoundry();

            Configuration = builder.Build();

            // var serviceInfos = CloudFoundryServiceInfoCreator.Instance(Configuration);
            // SsoServiceInfo = serviceInfos.GetServiceInfos<SsoServiceInfo>().FirstOrDefault()
            //                ?? throw new NullReferenceException("Service info for an SSO Provider was not found!");
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddConsole(LogLevel.Trace);
        }

        public static string GetContentRoot()
        {
            var basePath = (string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") ??
               AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }
    }
}