﻿using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
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
            _logger = logFactory.CreateLogger<FortuneService>();
            _httpClient = httpClient;
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            var response = await _httpClient.GetAsync(RANDOM_FORTUNE_URL);
            var result = Decode(await response.Content.ReadAsStreamAsync());
            _logger.LogInformation("RandomFortuneAsync: {0}", result.Text);
            return result;
        }

        private Fortune Decode(Stream stream)
        {
            try
            {
                var result = JsonSerializer.DeserializeAsync<Fortune>(stream);

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
