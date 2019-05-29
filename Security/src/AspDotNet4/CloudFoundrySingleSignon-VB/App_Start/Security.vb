Imports System.IdentityModel.Claims
Imports System.Web.Helpers
Imports Microsoft.Owin
Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.Cookies
Imports Owin
Imports Steeltoe.Security.Authentication.CloudFoundry.Owin

Namespace CloudFoundrySingleSignon

    Public Class Security
        Public Shared Sub ConfigureAuth(ByVal app As IAppBuilder)
            app.SetDefaultSignInAsAuthenticationType("ExternalCookie")
            app.UseCookieAuthentication(New CookieAuthenticationOptions With {
                .AuthenticationType = "ExternalCookie",
                .AuthenticationMode = AuthenticationMode.Active,
                .CookieName = ".AspNet.ExternalCookie",
                .LoginPath = New PathString("/Account/AuthorizeSSO"),
                .ExpireTimeSpan = TimeSpan.FromMinutes(5)
            })
            app.UseCloudFoundryOpenIdConnect(ApplicationConfig.Configuration)

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier
        End Sub
    End Class
End Namespace