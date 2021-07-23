using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FortuneTeller.UI.Services
{
    public class FortuneService : IFortuneService
    {
        private const string RANDOM_FORTUNE_URL = "random";
        private readonly HttpClient _httpClient;
        private readonly ILogger<FortuneService> _logger;

        public FortuneService(HttpClient httpClient, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<FortuneService>();
            _httpClient = httpClient;
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<Fortune>(RANDOM_FORTUNE_URL);
            _logger.LogInformation("RandomFortuneAsync: {0}", result.Text);
            return result;
        }
    }
}