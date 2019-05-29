using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
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

        public async Task<Fortune> RandomFortuneAsync()
        {
            var str = await _httpClient.GetStreamAsync(RANDOM_FORTUNE_URL);
            Fortune result = Decode(str);
            _logger.LogInformation("RandomFortuneAsync: {0}", result.Text);
            return result;
        }

        private Fortune Decode(Stream stream)
        {
            try
            {
                var serializer = new JsonSerializer();

                using (var sr = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(sr))
                    {
                        return serializer.Deserialize<Fortune>(jsonTextReader);
                    }
                }
            }
            catch (Exception e)
            {
                _logger?.LogError("Error {0} deserializing", e);
            }

            return new Fortune()
            {
                Id = 0,
                Text = "Have a good day!"
            };
        }
    }
}