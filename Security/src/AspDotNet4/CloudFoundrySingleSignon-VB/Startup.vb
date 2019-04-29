Imports Microsoft.Owin
Imports Owin

<Assembly: OwinStartup(GetType(CloudFoundrySingleSignon.Startup))>
Namespace CloudFoundrySingleSignon

    Public Class Startup
        Public Sub Configuration(ByVal app As IAppBuilder)
            ApplicationConfig.RegisterConfig("development")
            Security.ConfigureAuth(app)
        End Sub

    End Class
End Namespace
