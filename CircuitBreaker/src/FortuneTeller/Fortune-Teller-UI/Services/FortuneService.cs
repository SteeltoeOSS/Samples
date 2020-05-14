using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Steeltoe.Common.Discovery;

namespace Fortune_Teller_UI.Services
{

    public class FortuneService :  IFortuneService
    {
        DiscoveryHttpClientHandler _handler;
        ILogger<FortuneService> _logger;
        private const string RANDOM_FORTUNE_URL = "https://fortuneService/api/fortunes/random";
        private const string FORTUNES_URL = "https://fortuneService/api/fortunes";

        public FortuneService(IDiscoveryClient client, ILoggerFactory logFactory) 
        {
            _handler = new DiscoveryHttpClientHandler(client, logFactory.CreateLogger<DiscoveryHttpClientHandler>());
            // Remove comment to use SSL communications with Self-Signed Certs
            // _handler.ServerCertificateCustomValidationCallback = (a,b,c,d) => {return true;};
            this._logger = logFactory.CreateLogger<FortuneService>();
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            var client = GetClient();
            var result = await client.GetStringAsync(RANDOM_FORTUNE_URL);
            _logger.LogInformation("RandomFortuneAsync: {0}", result);

            return JsonConvert.DeserializeObject<Fortune>(result);
        }

        public async Task<List<Fortune>> GetFortunesAsync(List<int> fortuneIds)
        {
            var client = GetClient();
            var queryString = BuildQueryString(fortuneIds);
            var requestUrl = FORTUNES_URL + queryString;
            _logger.LogInformation("GetFortunesAsync issuing {0}", requestUrl);

            var result = await client.GetStringAsync(requestUrl);
            _logger.LogInformation("GetFortunesAsync returned {0}", result);

            return JsonConvert.DeserializeObject<List<Fortune>>(result);

        }

        private string BuildQueryString(List<int> ids)
        {
            if (ids.Count == 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder("?");
            foreach(var id in ids)
            {
                sb.Append("Ids=" + id.ToString());
                sb.Append("&");
            }
            return sb.ToString(0, sb.Length - 1);
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
    }
}
