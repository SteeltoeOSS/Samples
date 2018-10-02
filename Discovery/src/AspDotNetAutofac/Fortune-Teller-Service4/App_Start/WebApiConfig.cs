using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FortuneTellerService4
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
      
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}",
                defaults: new { controller = "Fortunes", action = "Get" }
        
            );
            config.Routes.MapHttpRoute(
                name: "RandomApi",
                routeTemplate: "api/{controller}/random",
          defaults: new { controller = "Fortunes", action = "Random" }
            );
        }
    }
}
