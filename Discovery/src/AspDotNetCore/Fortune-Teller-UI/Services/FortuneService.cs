using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{
    public class FortuneService : IFortuneService
    {
        ILogger<FortuneService> _logger;
        private const string RANDOM_FORTUNE_URL = "random";
        private readonly HttpClient _httpClient;

        public FortuneService(HttpClient httpClient, ILoggerFactory logFactory) 
        {
            _logger =  logFactory.CreateLogger<FortuneService>();
            _httpClient = httpClient;
        }

        public async Task<string> RandomFortuneAsync()
        {
            var result = await _httpClient.GetStringAsync(RANDOM_FORTUNE_URL);
            _logger.LogInformation("RandomFortuneAsync: {0}", result);
            return result;
        }
    }
}