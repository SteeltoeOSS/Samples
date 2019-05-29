using Microsoft.Extensions.Logging;
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
            var startLogger = ApplicationConfig.LoggerFactory.CreateLogger("Startup");
            var serviceInfos = CloudFoundryServiceInfoCreator.Instance(ApplicationConfig.Configuration);
            var ssoInfo = serviceInfos.GetServiceInfos<SsoServiceInfo>().FirstOrDefault() 
                ?? throw new NullReferenceException("Couldn't find SSO Service Info");

            startLogger.LogInformation("Listening at http://{uri}", ssoInfo.ApplicationInfo.ApplicationUris.First());
            var serviceHost = new ServiceHost(typeof(ValueService), new Uri("http://" + ssoInfo.ApplicationInfo.ApplicationUris.First()));
            serviceHost.AddJwtAuthorization(ApplicationConfig.Configuration, null, ApplicationConfig.LoggerFactory);
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