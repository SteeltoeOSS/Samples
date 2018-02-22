using Microsoft.Extensions.Options;
using Steeltoe.CloudFoundry.Connector.OAuth;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OAuth4.Controllers
{
    public class HomeController : Controller
    {
        OAuthServiceOptions _options;

        public HomeController(IOptions<OAuthServiceOptions> oauthOptions)
        {
            _options = oauthOptions.Value;
        }

        public ActionResult Index()
        {
            _options.Scope = new List<string> { "a", "b", "c", "d" };
            return View(_options ?? new OAuthServiceOptions());
        }
    }
}