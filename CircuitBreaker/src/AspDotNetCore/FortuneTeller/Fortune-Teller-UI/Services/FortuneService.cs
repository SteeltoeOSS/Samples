using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{

    public class FortuneService :  IFortuneService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        private ILogger<FortuneService> _logger;
        private const string RANDOM_FORTUNE_URL = "random";

        public FortuneService(IHttpClientFactory httpClientFactory, HttpClient httpClient, ILoggerFactory logFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;

            _logger = logFactory.CreateLogger<FortuneService>();
        }

        // ------- begin injected http client --------
        public async Task<Fortune> RandomFortuneAsync()
        {
            var result = await _httpClient.GetAsync(RANDOM_FORTUNE_URL);
            _logger.LogInformation("RandomFortuneAsync: {randomFortune}", await result.Content.ReadAsStringAsync());

            return await result.Content.ReadAsAsync<Fortune>();
        }

        public async Task<List<Fortune>> GetFortunesAsync(List<int> fortuneIds)
        {
            var queryString = BuildQueryString(fortuneIds);
            _logger.LogInformation("GetFortunesAsync with {querystring}", queryString);

            var result = await _httpClient.GetAsync(queryString);
            _logger.LogInformation("GetFortunesAsync returned {fortuneResult}", result);

            return await result.Content.ReadAsAsync<List<Fortune>>();
        }
        // ------- end injected http client --------

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

        // --------- begin injected factory --------
        // use HttpClientFactory to get a named client pre-configured with handler pipeline 
        // see startup.cs for configuration of these pipelines
        public async Task<Fortune> RandomFortuneWithRetryAsync()
        {
            return await GetClientDoWork(HttpClients.WithRetry);
        }

        public async Task<Fortune> RandomFortuneUserCommandAsync()
        {
            return await GetClientDoWork(HttpClients.WithUserCommand);
        }

        public async Task<Fortune> RandomFortuneDefaultCommandAsync()
        {
            return await GetClientDoWork(HttpClients.WithInlineCommand);
        }

        private async Task<Fortune> GetClientDoWork(string ClientName)
        {
            var client = _httpClientFactory.CreateClient(ClientName);
            var httpResponse = await client.GetAsync("random");
            return await httpResponse.Content.ReadAsAsync<Fortune>();
        }
        // ----------- end injected factory ---------
    }
}
