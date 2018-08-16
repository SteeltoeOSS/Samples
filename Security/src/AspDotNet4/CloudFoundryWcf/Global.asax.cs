using Steeltoe.Security.Authentication.CloudFoundry.Wcf;
using System;
using System.ServiceModel;

namespace CloudFoundryWcf
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ApplicationConfig.RegisterConfig("development");
            var serviceHost = new ServiceHost(typeof(ValueService));
            serviceHost.Authorization.ServiceAuthorizationManager = new JwtAuthorizationManager(new CloudFoundryOptions(ApplicationConfig.Configuration));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}