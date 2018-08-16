using CloudFoundrySingleSignon.App_Start;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Security.Authentication.CloudFoundry.Owin;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Helpers;

namespace CloudFoundrySingleSignon
{
    public partial class Startup
	{
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType("ExternalCookie");
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ExternalCookie",
                AuthenticationMode = AuthenticationMode.Active,
                CookieName = ".AspNet.ExternalCookie",
                LoginPath = new PathString("/Account/AuthorizeSSO"),
                ExpireTimeSpan = TimeSpan.FromMinutes(5)
            });

            var serviceInfos = CloudFoundryServiceInfoCreator.Instance(ApplicationConfig.Configuration);
            var ssoInfo = serviceInfos.GetServiceInfos<SsoServiceInfo>().FirstOrDefault()
                            ?? throw new NullReferenceException("Service info for an SSO Provider was not found!");

            app.UseOpenIDConnect(new OpenIDConnectOptions()
            {
                ClientID = ssoInfo.ClientId,
                ClientSecret = ssoInfo.ClientSecret,
                AuthDomain = ssoInfo.AuthDomain,
                AppHost = ssoInfo.ApplicationInfo.ApplicationUris.First(),
                AppPort = 0,
                AdditionalScopes = "testgroup",
                ValidateCertificates = false,
                CallbackPath = new PathString("/signin-cloudfoundry")
            });

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
	}
}