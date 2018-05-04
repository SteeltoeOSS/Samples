using System.Web;
using System.Web.Http;

namespace CloudFoundryJwtAuthentication
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ApplicationConfig.RegisterConfig("development");
        }
    }
}
