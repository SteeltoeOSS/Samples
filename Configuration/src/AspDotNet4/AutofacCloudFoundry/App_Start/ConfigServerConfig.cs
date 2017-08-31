using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using PA = Microsoft.Extensions.PlatformAbstractions;
using Pivotal.Extensions.Configuration;
using Pivotal.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AutofacCloudFoundry
{
    public static class ConfigServerConfig
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void RegisterConfig(string environment)
        {
            ILoggerFactory factory = new LoggerFactory();
            factory.AddProvider(new ConsoleLoggerProvider((category, logLevel) => logLevel >= LogLevel.Debug, false));

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
                .AddConfigServer(env, factory);

            Configuration = builder.Build();
        }

        public static void RegisterConfigServer(this ContainerBuilder container, IConfigurationRoot config)
        {
            container.RegisterOptions();
            container.Configure<ConfigServerClientSettingsOptions>(config);
            container.Configure<CloudFoundryApplicationOptions>(config);
            container.Configure<CloudFoundryServicesOptions>(config);
            container.RegisterInstance(config).As<IConfigurationRoot>().SingleInstance();
        }

        public static void RegisterOptions(this ContainerBuilder container)
        {
            container.RegisterGeneric(typeof(OptionsManager<>)).As(typeof(IOptions<>)).SingleInstance();
            container.RegisterGeneric(typeof(OptionsMonitor<>)).As(typeof(IOptionsMonitor<>)).SingleInstance();
            container.RegisterGeneric(typeof(OptionsSnapshot<>)).As(typeof(IOptionsSnapshot<>)).InstancePerRequest();
        }

        public static void Configure<TOption>(this ContainerBuilder container, IConfigurationRoot config) where TOption : class
        {
            container.RegisterInstance(new ConfigurationChangeTokenSource<TOption>(config)).As<IOptionsChangeTokenSource<TOption>>().SingleInstance();
            container.RegisterInstance(new ConfigureFromConfigurationOptions<TOption>(config)).As<IConfigureOptions<TOption>>().SingleInstance();
        }
    }
    public class HostingEnvironment : IHostingEnvironment
    {
        public HostingEnvironment(string env)
        {
            EnvironmentName = env;
            ApplicationName = PA.PlatformServices.Default.Application.ApplicationName;
            ContentRootPath = PA.PlatformServices.Default.Application.ApplicationBasePath;
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
