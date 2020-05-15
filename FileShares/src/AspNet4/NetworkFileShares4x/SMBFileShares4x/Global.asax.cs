using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SMBFileShares4x
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected static void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ApplicationConfig.Configure("development");
        }
    }
}
