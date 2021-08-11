using Steeltoe.Actuators.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Steeltoe.Actuators.Services
{
    public class LogLevelService : ILogLevelService
    {
        private readonly HttpClient httpClient;

        public LogLevelService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<DynamicLogLevels> GetLogLevelsAndNamespaces() =>
            await httpClient.GetFromJsonAsync<DynamicLogLevels>("actuator/loggers");

        public async Task<DynamicLogLevel> SetLogLevels(string name, string level)
        {
            var newLevel = new DynamicLogLevel { ConfiguredLevel = level };

            var response = await httpClient.PostAsJsonAsync($"actuator/loggers/{name}", newLevel);

            return (response.IsSuccessStatusCode) ? newLevel : default;
        }
    }
}
