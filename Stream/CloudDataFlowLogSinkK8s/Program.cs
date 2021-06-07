using Steeltoe.Common.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Configuration.Placeholder;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Extensions;
using Steeltoe.Stream.Messaging;
using Steeltoe.Management.Endpoint;

namespace CloudDataflowSink
{
    [EnableBinding(typeof(ISink))]
    public class Program
    {
        private static ILogger<Program> _logger;
        public static void Main(string[] args)
        {
            var host = CreateStreamHostBuilder(args).Build();
            _logger = host.Services.GetService<ILogger<Program>>();
       
            host.Run();
        }

        public static IHostBuilder CreateStreamHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
            .AddPlaceholderResolver()
            .ConfigureWebHostDefaults(webhostBuilder => webhostBuilder.UseStartup<Startup>())
            .UseCloudHosting()
            .AddCloudFoundryConfiguration()
            .AddAllActuators()
            .AddStreamServices<Program>();

        [StreamListener(ISink.INPUT)]
        public void HandleMessage(string input)
        {
            _logger.LogInformation("sink: "+input);
        }
    }
}
