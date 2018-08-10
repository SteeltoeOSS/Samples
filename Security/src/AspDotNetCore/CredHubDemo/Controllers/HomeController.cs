using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        private ICloudFoundrySettingsReader _cfSettings = new CloudFoundryEnvironmentSettingsReader();
        private static ICredHubClient _credHub;

        public HomeController(ILogger<HomeController> logger, ILoggerFactory loggerFactory, IOptionsSnapshot<CredHubOptions> credHubOptions)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            if (_credHub == null && Request?.Path.Value.Contains("Injected") != true)
            {
                try
                {
                    _logger?.LogTrace("Getting CredHub UAA Client...");
                    _credHub = CredHubClient.CreateUAAClientAsync(credHubOptions.Value, _loggerFactory.CreateLogger<CredHubClient>()).Result;
                }
                catch (Exception e)
                {
                    _logger?.LogCritical(e, "Failed to initialize CredHubClient");
                    throw new Exception($"Failed initializing CredHubClient: {e}");
                }
            }
        }

        public async Task<IActionResult> Index()
        {
            _logger?.LogTrace("Starting Home/Index Action...");
            try
            {
                var newPassword = await _credHub.WriteAsync<PasswordCredential>(new PasswordSetRequest("writtenPassword", Guid.NewGuid().ToString()));
                ViewBag.Deleted = await _credHub.DeleteByNameAsync("writtenPassword");

                return View(newPassword);
            }
            catch (Exception e)
            {
                throw new Exception($"Failed interacting with CredHub: {e}");
            }
        }

        public async Task<IActionResult> Injected([FromServices]ICredHubClient credHub)
        {
            var newPassword = await credHub.GenerateAsync<PasswordCredential>(new PasswordGenerationRequest("generated-password", new PasswordGenerationParameters { Length = 20 }, null, OverwiteMode.overwrite));
            ViewBag.Deleted = await credHub.DeleteByNameAsync("generated-password");

            return View(newPassword);
        }

        // interpolate VCAP_SERVICES data on-demand
        public async Task<IActionResult> Interpolate()
        {
            _logger.LogTrace("Creating CredHub Client...");

            var creds = "{\"key\": 123,\"key_list\": [\"val1\",\"val2\"],\"is_true\": true}";
            _logger.LogTrace("Setting credentials...");
            await _credHub.WriteAsync<JsonCredential>(new JsonSetRequest($"/credhubdemo-config-server/credentials", creds, null, OverwiteMode.overwrite));

            _logger.LogTrace("Setting up ViewModel and calling Interpolate...");
            var interpolated = await _credHub.InterpolateServiceDataAsync(Program.OriginalServices);
            var viewModel = new Dictionary<string, string>
            {
                { "PUT to CredHub at /config-server/credentials", creds },
                { "original", Program.OriginalServices },
                { "interpolated", JsonConvert.SerializeObject(JsonConvert.DeserializeObject(interpolated), Formatting.Indented) }
            };

            return View(viewModel);
        }

        // get VCAP_SERVICES as interpolated during startup
        public IActionResult GetInterpolatedServices()
        {
            var viewModel = new Dictionary<string, string>
            {
                { "original", Program.OriginalServices },
                { "interpolated", JsonConvert.SerializeObject(JsonConvert.DeserializeObject(_cfSettings.ServicesJson), Formatting.Indented) }
            };

            return View("Interpolate", viewModel);
        }

        /// <summary>
        /// Test all interactions with CredHub, grouped by credential type
        /// </summary>
        /// <returns>pass/fail</returns>
        public async Task<IActionResult> TestItAll()
        {
            // values
            var setValue = await _credHub.WriteAsync<ValueCredential>(new ValueSetRequest("testWriteValue", "test"));
            var getValueById = await _credHub.GetByIdAsync<ValueCredential>(setValue.Id);
            var getValueByName = await _credHub.GetByNameAsync<ValueCredential>(setValue.Name);
            var getValueByNameWithHistory = await _credHub.GetByNameWithHistoryAsync<ValueCredential>(setValue.Name, 2);
            var deleteValue = await _credHub.DeleteByNameAsync(setValue.Name);

            // JSON
            var setJson = await _credHub.WriteAsync<JsonCredential>(new JsonSetRequest("testWriteJson", "{'someKey':'someValue'}"));
            var getJsonById = await _credHub.GetByIdAsync<JsonCredential>(setJson.Id);
            var getJsonByName = await _credHub.GetByNameAsync<JsonCredential>(setJson.Name);
            var getJsonByNameWithHistory = await _credHub.GetByNameWithHistoryAsync<JsonCredential>(setJson.Name, 2);
            var deleteJson = await _credHub.DeleteByNameAsync(setJson.Name);

            // passwords
            var generatedPassword = await _credHub.GenerateAsync<PasswordCredential>(new PasswordGenerationRequest("generatedPassword", new PasswordGenerationParameters { Length = 20 }));
            var setPassword = await _credHub.WriteAsync<PasswordCredential>(new PasswordSetRequest("testWritePassword", generatedPassword.Value.ToString()));
            var getPasswordById = await _credHub.GetByIdAsync<PasswordCredential>(setPassword.Id);
            var getPasswordByName = await _credHub.GetByNameAsync<PasswordCredential>(setPassword.Name);
            var getPasswordByNameWithHistory = await _credHub.GetByNameWithHistoryAsync<PasswordCredential>(setPassword.Name, 2);
            var deleteGenPassword = await _credHub.DeleteByNameAsync(generatedPassword.Name);
            var deletePassword = await _credHub.DeleteByNameAsync(setPassword.Name);

            // certificates
            // generate a CA
            var generatedCA = await _credHub.GenerateAsync<CertificateCredential>(new CertificateGenerationRequest("testGeneratedCA", new CertificateGenerationParameters { IsCertificateAuthority = true, CommonName = "generator.ca" }));
            // generate a cert signed by the generated CA
            var generatedCert = await _credHub.GenerateAsync<CertificateCredential>(new CertificateGenerationRequest("testGeneratedCert", new CertificateGenerationParameters { CommonName = "generated.cert", CertificateAuthority = "/testGeneratedCA" }));
            // write the generated cert under another name
            var setCertificate = await _credHub.WriteAsync<CertificateCredential>(new CertificateSetRequest("testWriteCertificate", generatedCert.Value.PrivateKey, generatedCert.Value.Certificate, certificateAuthorityName: "/testGeneratedCA"));
            var getCertificateById = await _credHub.GetByIdAsync<CertificateCredential>(setCertificate.Id);
            var getCertificateByName = await _credHub.GetByNameAsync<CertificateCredential>(setCertificate.Name);
            var regenerateCert = await _credHub.RegenerateAsync<CertificateCredential>(generatedCert.Name);
            // regenerate both credentials signed by the CA
            var bulkRegenerate = await _credHub.BulkRegenerateAsync(generatedCA.Name);
            var getCertificateByNameWithHistory = await _credHub.GetByNameWithHistoryAsync<CertificateCredential>(generatedCert.Name, 3);
            var deleteCertificate = await _credHub.DeleteByNameAsync(setCertificate.Name);
            var deleteGenCert = await _credHub.DeleteByNameAsync(generatedCert.Name);
            var deleteCA = await _credHub.DeleteByNameAsync(generatedCA.Name);

            // RSA
            var generatedRsa = await _credHub.GenerateAsync<RsaCredential>(new RsaGenerationRequest("generatedRSA"));
            var setRsa = await _credHub.WriteAsync<RsaCredential>(new RsaSetRequest("testWriteRsa", generatedRsa.Value.PrivateKey, generatedRsa.Value.PublicKey));
            var getRsaById = await _credHub.GetByIdAsync<RsaCredential>(setRsa.Id);
            var getRsaByName = await _credHub.GetByNameAsync<RsaCredential>(setRsa.Name);
            var regeneratedRsa = await _credHub.RegenerateAsync<RsaCredential>(generatedRsa.Name);
            var getRsaByNameWithHistory = await _credHub.GetByNameWithHistoryAsync<RsaCredential>(generatedRsa.Name, 2);
            var deleteRsa = await _credHub.DeleteByNameAsync(setRsa.Name);
            var deleteGeneratedRsa = await _credHub.DeleteByNameAsync(generatedRsa.Name);

            // SSH
            var generatedSsh = await _credHub.GenerateAsync<SshCredential>(new SshGenerationRequest("generatedSsh", new SshGenerationParameters { SshComment = "this is a comment"}));
            var setSsh = await _credHub.WriteAsync<SshCredential>(new SshSetRequest("testWriteSsh", generatedSsh.Value.PublicKey, generatedSsh.Value.PublicKey));
            var getSshById = await _credHub.GetByIdAsync<SshCredential>(setSsh.Id);
            var getSshByName = await _credHub.GetByNameAsync<SshCredential>(setSsh.Name);
            var regeneratedSsh = await _credHub.RegenerateAsync<SshCredential>(generatedSsh.Name);
            var getSshByNameWithHistory = await _credHub.GetByNameWithHistoryAsync<SshCredential>(generatedSsh.Name, 2);
            var deleteSsh = await _credHub.DeleteByNameAsync(setSsh.Name);
            var deleteGeneratedSsh = await _credHub.DeleteByNameAsync(generatedSsh.Name);

            // User
            var generatedUser = await _credHub.GenerateAsync<UserCredential>(new UserGenerationRequest("generatedUser", new UserGenerationParameters()));
            var regeneratedUser = await _credHub.RegenerateAsync<UserCredential>("generatedUser");
            var deletedUser = await _credHub.DeleteByNameAsync(generatedUser.Name);

            // comment out one or more Delete operations above to see more results in Find requests:
            var paths = await _credHub.FindAllPathsAsync();
            foreach (var p in paths)
            {
                Console.WriteLine($"Found path {p.Path}");
            }
            var foundByPath = await _credHub.FindByPathAsync("/");
            foreach(var f in foundByPath)
            {
                Console.WriteLine($"Found credential {f.Name} by path");
            }
            var foundByName = await _credHub.FindByNameAsync("generated");
            foreach (var f in foundByName)
            {
                Console.WriteLine($"Found credential {f.Name} by name");
            }
            return Json("Look at Controllers/HomeController.cs to see what just completed successfully");
        }
    }
}
