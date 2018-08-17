using CloudFoundrySingleSignon.App_Start;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudFoundrySingleSignon
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // disable certificate validation
            ServicePointManager.ServerCertificateValidationCallback =
                    delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ApplicationConfig.RegisterConfig("development");
        }
    }
}
