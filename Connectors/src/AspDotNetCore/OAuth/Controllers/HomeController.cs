using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.CloudFoundry.Connector.OAuth;
using System.Collections.Generic;

namespace OAuth.Controllers
{
    public class HomeController : Controller
    {
        OAuthServiceOptions _options;

        public HomeController(IOptions<OAuthServiceOptions> oauthOptions)
        {
            _options = oauthOptions.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult OAuthOptions()
        {
            _options.Scope = new List<string> { "a", "b", "c", "d" };
            return View(_options ?? new OAuthServiceOptions());
        }
    }
}
