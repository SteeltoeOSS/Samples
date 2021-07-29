using Steeltoe.Actuators.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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

        public async Task<LogLevelsAndNamespaces> GetLogLevelsAndNamespaces() =>
            await httpClient.GetFromJsonAsync<LogLevelsAndNamespaces>("actuator/loggers");

        public async Task<LogNamespace> SetLogLevels(string name, string level)
        {
            var newLevel = new LogNamespace { ConfiguredLevel = level };

            var response = await httpClient.PostAsJsonAsync($"actuator/loggers/{name}", newLevel);

            return (response.IsSuccessStatusCode) ? newLevel : default;
        }
    }
}
