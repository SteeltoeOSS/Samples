using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Steeltoe.Common.Discovery;

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

        public async Task<Fortune> RandomFortuneAsync()
        {
            var client = GetClient();
            var str = await client.GetStreamAsync(RANDOM_FORTUNE_URL);
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

        private HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
    }
}
