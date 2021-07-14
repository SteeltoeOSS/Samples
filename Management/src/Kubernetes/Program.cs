using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Steeltoe.Bootstrap.Autoconfig;

namespace Kubernetes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            //create initialization logger factory(allow steeltoe components that runs during host initialization to log)
            var initLoggerFactory = new LoggerFactory().AddSerilog(new LoggerConfiguration()
                .MinimumLevel.Error().WriteTo.Console().CreateLogger());

            return builder
                //.AddKubernetesConfiguration(null, initLoggerFactory) //alternative that still does not work
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .AddSteeltoe(loggerFactory: initLoggerFactory);
        }
    }
}
