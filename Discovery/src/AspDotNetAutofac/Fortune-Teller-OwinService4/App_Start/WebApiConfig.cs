using Owin;
using System.Web.Http;

namespace FortuneTellerOwinService4
{
    public static class WebApiConfig
    {
        public static HttpConfiguration HttpConfig;

        public static HttpConfiguration RegisterRoutes(IAppBuilder app)
        {
            HttpConfig = new HttpConfiguration();

            HttpConfig.MapHttpAttributeRoutes();
            HttpConfig.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = "Get", id = RouteParameter.Optional }
            );

            app.UseWebApi(HttpConfig);

            return HttpConfig;
        }
    }
}
