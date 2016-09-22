using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.CloudFoundry.Connector.OAuth;
using System.Text;
using Microsoft.Extensions.Options;

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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult OAuthOptions()
        {
            if (_options != null)
            {
                ViewData["ClientId"] = _options.ClientId;
                ViewData["ClientSecret"] = _options.ClientSecret;
                ViewData["UserAuthorizationUrl"] = _options.UserAuthorizationUrl;
                ViewData["AccessTokenUrl"] = _options.AccessTokenUrl;
                ViewData["UserInfoUrl"] = _options.UserInfoUrl;
                ViewData["TokenInfoUrl"] = _options.TokenInfoUrl;
                ViewData["JwtKeyUrl"] = _options.JwtKeyUrl;
                ViewData["ValidateCertificates"] = _options.ValidateCertificates;
                ViewData["Scopes"] = CommanDelimit(_options.Scope);
            }
            else
            {
                ViewData["ClientId"] = "Not available";
                ViewData["ClientSecret"] = "Not available";
                ViewData["UserAuthorizationUrl"] = "Not available";
                ViewData["AccessTokenUrl"] = "Not available";
                ViewData["UserInfoUrl"] = "Not available";
                ViewData["TokenInfoUrl"] = "Not available";
                ViewData["JwtKeyUrl"] = "Not available";
                ViewData["ValidateCertificates"] = "Not available";
                ViewData["Scopes"] = "Not available";
            }
            return View();
        }

        private object CommanDelimit(ICollection<string> scope)
        {
            StringBuilder sb = new StringBuilder();
            bool comma = false;
            foreach (string s in scope)
            {
                if (comma)
                    sb.Append(",");
                sb.Append(s);
                comma = true;

            }
            return sb.ToString();
        }
    }
}
