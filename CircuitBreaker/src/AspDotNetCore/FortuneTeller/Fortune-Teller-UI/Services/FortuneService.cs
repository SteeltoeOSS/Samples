using Microsoft.Extensions.Logging;
using Pivotal.Discovery.Client;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{
    public class FortuneService : HystrixCommand<string>, IFortuneService
    {
        DiscoveryHttpClientHandler _handler;

        private const string RANDOM_FORTUNE_URL = "http://fortuneService/api/fortunes/random";
        ILogger<FortuneService> logger;

        public FortuneService(IHystrixCommandOptions options, IDiscoveryClient client, ILogger<FortuneService> logger) : base(options)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            IsFallbackUserDefined = true;
            this.logger = logger;
        }

        public async Task<string> RandomFortuneAsync()
        {
            return await ExecuteAsync();
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }

        protected override string Run()
        {
            var client = GetClient();
            string result =  client.GetStringAsync(RANDOM_FORTUNE_URL).Result;
            logger.LogInformation("Run: {0}", result);
            return result;
        }

        protected override string RunFallback()
        {
            logger.LogInformation("RunFallback");
            return "{\"id\":1,\"text\":\"You will have a happy day!\"}";
        }
    }
}
