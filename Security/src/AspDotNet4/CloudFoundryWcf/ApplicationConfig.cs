using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.IO;

namespace CloudFoundryWcf
{
    public class ApplicationConfig
    {
        public static IConfiguration Configuration { get; set; }
        public static LoggerFactory LoggerFactory { get; private set; }

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