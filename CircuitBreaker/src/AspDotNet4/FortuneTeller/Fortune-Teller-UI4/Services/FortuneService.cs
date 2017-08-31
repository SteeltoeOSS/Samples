using Pivotal.Discovery.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Steeltoe.CircuitBreaker.Hystrix;
using Microsoft.Extensions.Logging;

namespace FortuneTellerUI4.Services
{
    public class FortuneService : HystrixCommand<string>, IFortuneService
    {
        DiscoveryHttpClientHandler _handler;
        private const string RANDOM_FORTUNE_URL = "https://fortuneService/api/fortunes/random";
        private ILogger<FortuneService> _logger;

        public FortuneService(IHystrixCommandOptions options, IDiscoveryClient client, ILoggerFactory logFactory = null) : base(options)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            // Remove comment to use SSL communications with Self-Signed Certs
            // _handler.ServerCertificateCustomValidationCallback = (a,b,c,d) => {return true;};
            IsFallbackUserDefined = true;
            _logger = logFactory?.CreateLogger<FortuneService>();
        }

        public async Task<string> RandomFortuneAsync()
        {
            _logger?.LogInformation("RandomFortuneAsync");
            var result = await ExecuteAsync();
            return result;
        }


        protected override async Task<string> RunAsync()
        {
            _logger?.LogInformation("RunAsync");
            var client = GetClient();
            var result = await client.GetStringAsync(RANDOM_FORTUNE_URL);
            return result;
        }

        protected override async Task<string> RunFallbackAsync()
        {
            _logger?.LogInformation("RunFallbackAsync");
            return await Task.FromResult("{\"id\":1,\"text\":\"You will have a happy day!\"}");
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
    }
}
