using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Common;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.Kubernetes;
using Steeltoe.Management.Kubernetes;

namespace Kubernetes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddKubernetes(loggerFactory: GetLoggerFactory());
                })
                .AddKubernetesActuators();

        private static ILoggerFactory GetLoggerFactory()
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
