using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Discovery;
using Steeltoe.Discovery.Eureka;
using System.Threading;
using System.Threading.Tasks;

namespace Fetch
{
    internal class ConsoleFetchService : IHostedService
    {
        private readonly DiscoveryClient _discoveryClient;
        private readonly ILogger<ConsoleFetchService> _logger;

        public ConsoleFetchService(IDiscoveryClient discoveryClient, ILogger<ConsoleFetchService> logger)
        {
            _discoveryClient = discoveryClient as DiscoveryClient;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Get what applications have been fetched
            var apps = _discoveryClient.Applications;

            // Try to find app with name "MyApp", it is registered in the Register console application
            var app = apps.GetRegisteredApplication("MyApp");
            if (app != null)
            {
                // Print the instance info, and then exit loop
                _logger.LogInformation("Successfully fetched application: {0} ", app);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _discoveryClient.ShutdownAsync();
        }
    }
}