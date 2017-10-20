using Microsoft.Extensions.Logging;
using Steeltoe.Common.Discovery;
using System.Net.Http;
using System.Threading.Tasks;

namespace FortuneTellerUI4.Services
{
    public class FortuneService : IFortuneService
    {
        DiscoveryHttpClientHandler _handler;

        private const string RANDOM_FORTUNE_URL = "http://fortuneService/api/fortunes/random";
        private ILogger<FortuneService> _logger;

        public FortuneService(IDiscoveryClient client, ILoggerFactory logFactory = null)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            _logger = logFactory?.CreateLogger<FortuneService>();
        }

        public async Task<string> RandomFortuneAsync()
        {
            _logger?.LogInformation("RandomFortuneAsync");
            var client = GetClient();
            return await client.GetStringAsync(RANDOM_FORTUNE_URL);
 
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
    }
}
