using Pivotal.Discovery.Client;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Fortune_Teller_UI.Services
{
    public class FortuneService : IFortuneService
    {
        DiscoveryHttpClientHandler _handler;
        ILogger<FortuneService> _logger;
        private const string RANDOM_FORTUNE_URL = "http://fortuneService/api/fortunes/random";

        public FortuneService(IDiscoveryClient client, ILoggerFactory logFactory) 
        {
            _handler =  new DiscoveryHttpClientHandler(client, logFactory.CreateLogger<DiscoveryHttpClientHandler>());
            // Remove comment to use SSL communications with Self-Signed Certs
           // _handler.ServerCertificateCustomValidationCallback = (a,b,c,d) => {return true;};
            _logger =  logFactory.CreateLogger<FortuneService>();
        }

        public async Task<string> RandomFortuneAsync()
        {
            var client = GetClient();
            var result = await client.GetStringAsync(RANDOM_FORTUNE_URL);
            _logger.LogInformation("RandomFortuneAsync: {0}", result);
            return result;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
    }
}
