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
        public IActionResult About()
        {
            ViewData["Message"] = "Your About page.";
            return View();
        }


        [Authorize(Policy = "testgroup1")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        #endregion

        #region Token Delegation

        public async Task<IActionResult> InvokeJwtSample()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string values;
            var jwtSamplesUrl = GetSamplesUrl(HttpContext, JWTAPPS_HOSTNAME);

            try
            {
                values = await httpClient.GetStringAsync(jwtSamplesUrl + "/api/values");
            }
            catch (Exception e)
            {
                values = "Request failed: " + e.Message + ", expect JWT Sample app to be listening at: " + jwtSamplesUrl;
            }

            return View("InvokeService", values);
        }

        #endregion

        #region Client Certificate (Mutual TLS)

        public async Task<IActionResult> InvokeSameOrgSample()
        {
            var result = await httpClient.GetStringAsync(GetSamplesUrl(HttpContext, MTLS_HOSTNAME) + "/api/SameOrgCheck");
            return View("InvokeService", result);
        }

        public async Task<IActionResult> InvokeSameSpaceSample()
        {
            var result = await httpClient.GetStringAsync(GetSamplesUrl(HttpContext, MTLS_HOSTNAME) + "/api/SameSpaceCheck");
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
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
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

        const string JWTAPPS_HOSTNAME = "jwtauth";
        const string SSO_HOSTNAME = "single-signon";
        const string MTLS_HOSTNAME = "mtls-server";
        private string GetSamplesUrl(HttpContext httpContext, string serviceName)
        {
            var hostName = httpContext.Request.Host.Host;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var serviceHostname = hostName;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            var indx = hostName.IndexOf(SSO_HOSTNAME);
            if (indx >= 0)
            {
                var prefix = hostName.Substring(indx + 13, 0);
                var suffix = hostName.Substring(indx + 13, hostName.Length - indx - 13);
                serviceHostname = prefix + serviceName + suffix;
            }
            else
            {
                indx = hostName.IndexOf('.');
                if (indx < 0)
                {
                    serviceHostname = hostName + Services[serviceName];
                }
                else
                {
                    serviceHostname = serviceName + hostName.Substring(indx);
                }
            }

            return "https://" + serviceHostname;
        }

        private readonly Dictionary<string, string> Services = new Dictionary<string, string>
        {
            { JWTAPPS_HOSTNAME, ":8083" },
            { MTLS_HOSTNAME, ":8085" }
        };
    }
}
