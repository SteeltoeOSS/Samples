using CloudFoundrySingleSignon.App_Start;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudFoundrySingleSignon
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ApplicationConfig.RegisterConfig("development");
        }
    }
}
