Imports System.IO
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.Logging
Imports Steeltoe.Extensions.Configuration.CloudFoundry

Namespace CloudFoundrySingleSignon
    Public Class ApplicationConfig
        Public Shared Property Configuration As IConfiguration
        Public Shared Property LoggerFactory As LoggerFactory

        Public Shared Sub RegisterConfig(ByVal environment As String)
            Dim builder = New ConfigurationBuilder().SetBasePath(GetContentRoot()).AddJsonFile("appsettings.json", [optional]:=False, reloadOnChange:=False).AddEnvironmentVariables().AddCloudFoundry()
            Configuration = builder.Build()
            LoggerFactory = New LoggerFactory()
            LoggerFactory.AddConsole(LogLevel.Trace)
        End Sub

        Public Shared Function GetContentRoot() As String
            Dim basePath = If(CStr(AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY")), AppDomain.CurrentDomain.BaseDirectory)
            Return Path.GetFullPath(basePath)
        End Function
    End Class
End Namespace
