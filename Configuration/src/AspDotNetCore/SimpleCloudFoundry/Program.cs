using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using Pivotal.Extensions.Configuration.ConfigServer;

namespace SimpleCloudFoundry
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .UseCloudFoundryHosting()
                    .AddConfigServer(GetLoggerFactory())
                    .UseStartup<Startup>()
                    .Build();

        public static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace));
            serviceCollection.AddLogging(builder => builder.AddConsole((opts) =>
            {
                opts.DisableColors = true;
            }));
            serviceCollection.AddLogging(builder => builder.AddDebug());
            return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }
    }
}
