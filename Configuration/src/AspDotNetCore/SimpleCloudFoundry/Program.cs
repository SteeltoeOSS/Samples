using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
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
                    .ConfigureAppConfiguration(b => b.AddConfigServer(new LoggerFactory().AddConsole(LogLevel.Trace)))
                    .UseStartup<Startup>()
                    .Build();

    }
}
