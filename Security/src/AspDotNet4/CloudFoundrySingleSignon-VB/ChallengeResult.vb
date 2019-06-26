Imports Microsoft.Owin.Security
Imports System.Web
Imports System.Web.Mvc

Namespace CloudFoundrySingleSignon.Models
    Friend Class ChallengeResult
        Inherits HttpUnauthorizedResult

        Public Sub New(ByVal provider As String, ByVal redirectUri As String)
            LoginProvider = provider
            Me.RedirectUri = redirectUri
        End Sub

        Public Property LoginProvider As String
        Public Property RedirectUri As String

        Public Overrides Sub ExecuteResult(ByVal context As ControllerContext)
            Dim properties = New AuthenticationProperties With {
                .RedirectUri = RedirectUri
            }
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider)
        End Sub
    End Class
End Namespace
