using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Security.DataProtection.CredHub;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CredHubDemo.Controllers
{
    public class HomeController : Controller
    {
        private ILoggerFactory _loggerFactory;
        private ILogger _logger;
        private IConfiguration _config;
        private ICloudFoundrySettingsReader _cfSettings = new CloudFoundryEnvironmentSettingsReader();
        private ICredHubClient _credHub;

        public HomeController(ILogger<HomeController> logger, ILoggerFactory loggerFactory, IConfiguration configuration, ICredHubClient credHub)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _config = configuration;
            _credHub = credHub;
        }

        public async Task<IActionResult> Index()
        {
            var newPassword = await _credHub.GenerateAsync<PasswordCredential>(new PasswordGenerationRequest("generated-password", new PasswordGenerationParameters { Length = 20 }, true));
            ViewBag.Deleted = await _credHub.DeleteByNameAsync("generated-password");

            return View(newPassword);
        }

        public async Task<IActionResult> Interpolate()
        {
            _logger.LogTrace("Creating CredHub Client...");

            var creds = "{\"key\": 123,\"key_list\": [\"val1\",\"val2\"],\"is_true\": true}";
            _logger.LogTrace("Setting credentials...");
            await _credHub.WriteAsync<JsonCredential>(new JsonSetRequest("/config-server/credentials", creds, overwrite: true));

            _logger.LogTrace("Setting up ViewModel and calling Interpolate...");
            var interpolated = await _credHub.InterpolateServiceDataAsync(_cfSettings.ServicesJson);
            var viewModel = new Dictionary<string, string>
            {
                { "PUT to CredHub at /config-server/credentials", creds },
                { "original", _cfSettings.ServicesJson },
                { "interpolated", JsonConvert.SerializeObject(JsonConvert.DeserializeObject(interpolated), Formatting.Indented) }
            };

            return View(viewModel);
        }
    }
}
