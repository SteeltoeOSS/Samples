Imports System.Security.Claims

Namespace CloudFoundrySingleSignon
    Public Class AuthorizationManager
        Inherits ClaimsAuthorizationManager

        Public Overrides Function CheckAccess(ByVal context As AuthorizationContext) As Boolean
            Dim requiredClaims = context.Action.[Select](Function(v) v.Value)
            Dim possessedClaims = context.Principal.Claims.[Select](Function(v) v.Value)
            Dim claim = (CType(context.Principal.Identity, ClaimsIdentity)).FindFirst("testgroup")

            For Each c In requiredClaims

                If possessedClaims.Contains(c) Then
                    Return True
                End If
            Next

            Return False
        End Function
    End Class
End Namespace
