using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{

    public class FortuneService :  IFortuneService
    {
        private readonly HttpClient _httpClient;
        private ILogger<FortuneService> _logger;
        private const string RANDOM_FORTUNE_URL = "random";

        public FortuneService(HttpClient httpClient, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<FortuneService>();
            _httpClient = httpClient;
        }

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
    }
}
