using Apache.Geode.Client;
using Gemfire.Models;
using Pivotal.GemFire.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Gemfire
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ServerConfig.RegisterConfig("development");

            //SessionStateStore.AuthInitialize = new BasicAuthInitialize("developer_KfqcIiDwxAudQ6VvI7Snw", "QkALvzpFcFr5VNcF7Ax7A"); //pcfone
            //SessionStateStore.AuthInitialize = new BasicAuthInitialize("developer_zcs4XnFoWIDg14VVA7GKxA", "MGMtLoPDToFXlfnFhYZpA"); //beet.springapps.io
            //SessionStateStore.AuthInitialize = new BasicAuthInitialize("john", "secret");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }
    }
}
