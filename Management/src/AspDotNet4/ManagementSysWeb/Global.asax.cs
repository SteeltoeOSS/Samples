using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using ManagementSysWeb.App_Start;
using Steeltoe.Common.Configuration.Autofac;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.EndpointAutofac;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ManagementSysWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}