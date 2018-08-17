using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Security.Authentication.CloudFoundry.Wcf;
using System;
using System.Linq;
using System.ServiceModel;

namespace CloudFoundryWcf
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ApplicationConfig.RegisterConfig("development");
            var serviceInfos = CloudFoundryServiceInfoCreator.Instance(ApplicationConfig.Configuration);
            var ssoInfo = serviceInfos.GetServiceInfos<SsoServiceInfo>().FirstOrDefault() 
                ?? throw new NullReferenceException("Couldn't find SSO Service Info");

            var serviceHost = new ServiceHost(typeof(ValueService), new Uri("http://" + ssoInfo.ApplicationInfo.ApplicationUris.First()));
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