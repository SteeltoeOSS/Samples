Imports System.Web.Mvc
Imports System.Web.Routing
Imports Thinktecture.IdentityModel.Authorization.Mvc

Public Class CustomClaimsAuthorizeAttribute
    Inherits ClaimsAuthorizeAttribute

    Public Sub New(ByVal action As String, ParamArray resources As String())
        MyBase.New(action, resources)
    End Sub

    Protected Overrides Sub HandleUnauthorizedRequest(ByVal filterContext As AuthorizationContext)
        If filterContext.HttpContext.User.Identity.IsAuthenticated Then
            filterContext.Result = New RedirectToRouteResult(
                                    New RouteValueDictionary(New With {
                                        Key .controller = "Account",
                                        Key .action = "AccessDenied"
            }))
        Else
            MyBase.HandleUnauthorizedRequest(filterContext)
        End If
    End Sub
End Class
