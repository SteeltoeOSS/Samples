using Pivotal.Discovery.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Steeltoe.CircuitBreaker.Hystrix;

namespace FortuneTellerUI4.Services
{
    public class FortuneService : HystrixCommand<string>, IFortuneService
    {
        DiscoveryHttpClientHandler _handler;
        private const string RANDOM_FORTUNE_URL = "https://fortuneService/api/fortunes/random";


        public FortuneService(IHystrixCommandOptions options, IDiscoveryClient client) : base(options)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            // Remove comment to use SSL communications with Self-Signed Certs
            // _handler.ServerCertificateCustomValidationCallback = (a,b,c,d) => {return true;};
            IsFallbackUserDefined = true;
        }

        public async Task<string> RandomFortuneAsync()
        {
            var result = await ExecuteAsync();
            return result;
        }


        protected override async Task<string> RunAsync()
        {
            var client = GetClient();
            var result = await client.GetStringAsync(RANDOM_FORTUNE_URL);
            return result;
        }

        protected override async Task<string> RunFallbackAsync()
        {
            return await Task.FromResult("{\"id\":1,\"text\":\"You will have a happy day!\"}");
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
    }
    //public class FortuneService : IFortuneService
    //{
    //    DiscoveryHttpClientHandler _handler;

    //    private const string RANDOM_FORTUNE_URL = "http://fortuneService/api/fortunes/random";

    //    public FortuneService(IDiscoveryClient client)
    //    {
    //        _handler = new DiscoveryHttpClientHandler(client);
    //    }

    //    public async Task<string> RandomFortuneAsync()
    //    {
    //        var client = GetClient();
    //        return await client.GetStringAsync(RANDOM_FORTUNE_URL);
 
    //    }

    //    private HttpClient GetClient()
    //    {
    //        var client = new HttpClient(_handler, false);
    //        return client;
    //    }
    //}
}
