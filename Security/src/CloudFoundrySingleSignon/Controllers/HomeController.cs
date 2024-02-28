using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace CloudFoundrySingleSignon.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory clientFactory, ILogger<HomeController> logger)
        {
            httpClient = clientFactory.CreateClient("default");
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region SSO

        [Authorize(Policy = "testgroup")]
        public IActionResult TestGroup()
        {
            ViewData["Message"] = "You have the 'testgroup' permission.";
            return View();
        }


        [Authorize(Policy = "testgroup1")]
        public IActionResult AnotherTestGroup()
        {
            ViewData["Message"] = "You have the 'testgroup1' permission.";

            return View();
        }

        #endregion

        #region Token Delegation

        public async Task<IActionResult> InvokeJwtSample()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string values;
            var jwtSamplesUrl = GetSamplesUrl(HttpContext, JwtAppHostname);

            try
            {
                values = await httpClient.GetStringAsync($"{jwtSamplesUrl}/api/values");
            }
            catch (Exception e)
            {
                values = $"Request failed: {e.Message}, looked for JWT Sample app at: {jwtSamplesUrl}";
            }

            return View("InvokeService", values);
        }

        #endregion

        #region Client Certificate (Mutual TLS)

        public async Task<IActionResult> InvokeSameOrgSample()
        {
            string result;
            var mTlsSampleUrl = GetSamplesUrl(HttpContext, MutualTlsAppHostname);
            try
            {
                result = await httpClient.GetStringAsync($"{mTlsSampleUrl}/api/SameOrgCheck");
            }
            catch (Exception e)
            {
                result = $"Request failed: {e.Message}, looked for Mutual TLS Sample app at: {mTlsSampleUrl}";
            }

            return View("InvokeService", result);
        }

        public async Task<IActionResult> InvokeSameSpaceSample()
        {
            string result;
            var mTlsSampleUrl = GetSamplesUrl(HttpContext, MutualTlsAppHostname);
            try
            {
                result = await httpClient.GetStringAsync($"{mTlsSampleUrl}/api/SameSpaceCheck");
            }
            catch (Exception e)
            {
                result = $"Request failed: {e.Message}, looked for Mutual TLS Sample app at: {mTlsSampleUrl}";
            }

            return View("InvokeService", result);
        }

        #endregion

        public IActionResult Error()
        {
            return View();
        }

        #region Account-related
        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Manage()
        {
            ViewData["Message"] = "Manage accounts using UAA or CF command line.";
            return View();
        }

        public IActionResult AccessDenied()
        {
            ViewData["Message"] = "Insufficient permissions.";
            return View();
        }
        #endregion

        private const string JwtAppHostname = "jwtauth";
        private const string MutualTlsAppHostname = "mtls-server";
        private const string SsoHostname = "single-signon";
        private string GetSamplesUrl(HttpContext httpContext, string serviceName)
        {
            var hostName = httpContext.Request.Host.Host;
            var serviceHostname = hostName;
            var indexOfHost = hostName.IndexOf(SsoHostname, StringComparison.Ordinal);
            if (indexOfHost >= 0)
            {
                var prefix = hostName.Substring(indexOfHost + 13, 0);
                var suffix = hostName.Substring(indexOfHost + 13, hostName.Length - indexOfHost - 13);
                serviceHostname = prefix + serviceName + suffix;
            }
            else
            {
                indexOfHost = hostName.IndexOf('.');
                if (indexOfHost < 0)
                {
                    serviceHostname = hostName + _services[serviceName];
                }
                else
                {
                    serviceHostname = $"{serviceName}{hostName[indexOfHost..]}";
                }
            }

            return "https://" + serviceHostname;
        }

        private readonly Dictionary<string, string> _services = new()
        {
            { JwtAppHostname, ":8083" },
            { MutualTlsAppHostname, ":8085" }
        };
    }
}
