using CloudFoundrySingleSignon.App_Start;
using Steeltoe.Security.Authentication.CloudFoundry.Wcf;
using System;
using System.IdentityModel.Claims;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
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
            ViewBag.Title = "Steeltoe Legacy ASP.NET Security Samples";
            return View();
        }

        [Authorize]
        public ActionResult Secure()
        {
            ViewBag.Title = "Steeltoe Legacy ASP.NET Security Samples";
            ViewBag.Message = "You're now logged in as " + User.Identity.Name;
            return View("Index");
        }

        [CustomClaimsAuthorize("testgroup")]
        public ActionResult TestGroup()
        {
            ViewBag.Title = "Steeltoe Legacy ASP.NET Security Samples";
            ViewBag.Message = "Congratulations, you have access to 'testgroup'";
            return View("Index");
        }

        [CustomClaimsAuthorize("testgroup1")]
        public ActionResult TestGroup1()
        {
            return View("Index");
        }

        [Authorize]
        public async Task<ActionResult> TestJwtSample()
        {
            Console.WriteLine("Recieved authorized request to test REST + JWT Sample");
            ViewBag.Title = "Test JWT Sample";

            var token = Request.GetOwinContext().Authentication.User.Claims.First(c => c.Type == ClaimTypes.Authentication)?.Value;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string jwtSamplesUrl = GetServiceUrl(HttpContext, "jwt") + "/api/values";

            try
            {
                ViewBag.Message = await client.GetStringAsync(jwtSamplesUrl);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("401 (Unauthorized)"))
                {
                    ViewBag.Message = "Request failed, you are not authorized!";
                }
                else
                {
                    ViewBag.Message = "Request failed: " + e.Message + " expect JWT Sample app to be listening at: " + jwtSamplesUrl;
                }
            }
            return View("Index");
        }

        [Authorize]
        public async Task<ActionResult> TestWcfSample()
        {
            Console.WriteLine("Recieved authorized request to test WCF + JWT Sample");
            ViewBag.Title = "Test WCF Sample";
            var token = Request.GetOwinContext().Authentication.User.Claims.First(c => c.Type == ClaimTypes.Authentication)?.Value;

            // Specify the address to be used for the client.
            BasicHttpsBinding binding = new BasicHttpsBinding();
            EndpointAddress address = new EndpointAddress(GetServiceUrl(HttpContext, "wcf") + "/valueservice.svc");
            var sRef = new ValueService.ValueServiceClient(binding, address);
            sRef.Endpoint.EndpointBehaviors.Add(new JwtHeaderEndpointBehavior(new CloudFoundryOptions(ApplicationConfig.Configuration), token));
            try
            {
                ViewBag.Message = await sRef.GetDataAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                if (e.Message == "Unauthorized")
                {
                    ViewBag.Message = "Request failed, you are not authorized!";
                }
                else
                {
                    ViewBag.Message = e.Message;
                }
            }

            return View("Index");
        }

        const string SSO_HOSTNAME = "single-signon";
        const string JWT_HOSTNAME = "jwtauth";
        const string WCF_HOSTNAME = "wcf-jwt";

        private string GetServiceUrl(HttpContextBase httpContext, string service)
        {
            string hostName = httpContext.Request.Url.Host;

            int indx = hostName.IndexOf(SSO_HOSTNAME);

            // if on cloud foundry...
            if (hostName.Contains(SSO_HOSTNAME))
            {
                var suffix = hostName.Substring(indx + 13, hostName.Length - indx - 13);
                if (service == "jwt")
                {
                    hostName = JWT_HOSTNAME + suffix;
                }
                else
                {
                    hostName = WCF_HOSTNAME + suffix;
                }
            }
            else
            {
                if (service == "jwt")
                {
                    hostName = "localhost:44330";
                }
                else
                {
                    hostName = "localhost:44314";
                }
            }
            Console.WriteLine($"Resolved remote service hostname request for {service} as {hostName}");
            return "https://" + hostName;
        }
    }
}