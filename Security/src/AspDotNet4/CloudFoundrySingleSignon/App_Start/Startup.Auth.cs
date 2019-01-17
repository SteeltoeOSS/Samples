using CloudFoundrySingleSignon.App_Start;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Steeltoe.Security.Authentication.CloudFoundry.Owin;
using System;
using System.Security.Claims;
using System.Web.Helpers;

namespace CloudFoundrySingleSignon
{
    public partial class Startup
	{
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.Use((context, next) =>
            {
                if (context.Request.Headers["X-Forwarded-Proto"] == "https")
                {
                    context.Request.Scheme = "https";
                }

                return next();
            });

            app.SetDefaultSignInAsAuthenticationType("ExternalCookie");
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ExternalCookie",
                AuthenticationMode = AuthenticationMode.Active,
                CookieName = ".AspNet.ExternalCookie",
                LoginPath = new PathString("/Account/AuthorizeSSO"),
                ExpireTimeSpan = TimeSpan.FromMinutes(5)
            });

            app.UseCloudFoundryOpenIdConnect(ApplicationConfig.Configuration, "CloudFoundry", ApplicationConfig.LoggerFactory);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
	}
}