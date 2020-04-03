using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.ConfigServer;
using System;
using System.IO;

namespace AutofacCloudFoundry
{
    public static class ApplicationConfig
    {
        public static IConfiguration Configuration { get; set; }

        public static void RegisterConfig(string environment)
        {
            ILoggerFactory factory = new LoggerFactory();
            factory.AddConsole(LogLevel.Debug);

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(GetContentRoot())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()

                .AddConfigServer(environment, factory);

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
