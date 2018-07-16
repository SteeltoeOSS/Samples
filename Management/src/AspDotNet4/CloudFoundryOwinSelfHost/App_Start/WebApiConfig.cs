using Owin;
using System.Web.Http;
using System.Web.Http.Description;

namespace CloudFoundryOwinSelfHost
{
    public static class WebApiConfig
    {
        public static HttpConfiguration HttpConfig;

        public static void RegisterRoutes(IAppBuilder app)
        {
            HttpConfig = new HttpConfiguration();

            HttpConfig.MapHttpAttributeRoutes();
            HttpConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(HttpConfig);

            // if you don't do this, we won't get routes?
            HttpConfig.EnsureInitialized();
        }

        public static IApiExplorer GetApiExplorer()
        {
            return HttpConfig.Services.GetApiExplorer();
        }
    }
}
