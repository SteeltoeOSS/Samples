using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CloudFoundrySingleSignon.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult TestGroup()
        {
            return View();
        }

        [Authorize]
        public ActionResult TestGroup1()
        {
            return View();
        }

        public async Task<ActionResult> TestJwtSample()
        {
            //var token = await HttpContext.GetTokenAsync("access_token");
            //var token = await HttpContextAccessor.HttpContext.Authentication.GetTokenAsync("access_token");

            var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string response = null;
            string jwtSamplesUrl = GetJwtSamplesUrl(HttpContext);

            try
            {
                response = await client.GetStringAsync(jwtSamplesUrl);
            }
            catch (Exception e)
            {
                response = "Request failed: " + e.Message + " , expect JWT Sample app to be listening at: " + jwtSamplesUrl;
            }
            return View((object)response);
        }

        public async Task<ActionResult> TestWcfSample()
        {
            var sRef = new ValueServiceReference.ValueServiceClient();
            var response = await sRef.GetDataAsync();
            return View((object)response);
        }

        public ActionResult AccessDenied()
        {
            ViewData["Message"] = "Insufficient permissions.";
            return View();
        }

        const string JWTAPPS_HOSTNAME = "jwtauth";
        const string SSO_HOSTNAME = "single-signon";
        private string GetJwtSamplesUrl(HttpContextBase httpContext)
        {
            //string hostName = httpContext.Request.Url.Host;
            //string jwtappsHostname = hostName;
            //int indx = hostName.IndexOf(SSO_HOSTNAME);
            //if (indx >= 0)
            //{
            //    var prefix = hostName.Substring(indx + 13, 0);
            //    var suffix = hostName.Substring(indx + 13, hostName.Length - indx - 13);
            //    jwtappsHostname = prefix + JWTAPPS_HOSTNAME + suffix;
            //}
            //else
            //{
            //    indx = hostName.IndexOf('.');
            //    if (indx < 0)
            //    {
            //        return hostName;
            //    }
            //    jwtappsHostname = JWTAPPS_HOSTNAME + hostName.Substring(indx);
            //}
            //return "http://" + jwtappsHostname + "/api/values";
            return "http://localhost:23993/api/values";
        }
    }
}