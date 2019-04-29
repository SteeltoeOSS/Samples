Imports CloudFoundrySingleSignon_VB.CloudFoundrySingleSignon.Models
Imports Steeltoe.Security.Authentication.CloudFoundry
Imports System.Web
Imports System.Web.Mvc

Namespace CloudFoundrySingleSignon.Controllers
    Public Class AccountController
        Inherits Controller

        Public Function AuthorizeSSO(ByVal returnUrl As String) As ActionResult
            Return New ChallengeResult(CloudFoundryDefaults.AuthenticationScheme, If(returnUrl, Url.Action("Secure", "Home")))
        End Function

        Public Function AccessDenied() As ActionResult
            ViewData("Message") = "Insufficient permissions."
            Return View()
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken>
        Public Function LogOff() As ActionResult
            Request.GetOwinContext().Authentication.SignOut()
            Return RedirectToAction("Index", "Home")
        End Function
    End Class
End Namespace
