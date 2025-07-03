using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Certificates;
using Steeltoe.Security.Authorization.Certificate;

namespace Steeltoe.Samples.AuthApi.Controllers;

[Route("api/certificate/[action]")]
[ApiController]
public class CertificateAuthorizationController(IOptionsMonitor<CertificateOptions> certificateOptionsMonitor, ILogger<CertificateAuthorizationController> logger) : ControllerBase
{
    [HttpGet]
    public string GetCertificate()
    {
        CertificateOptions options = certificateOptionsMonitor.Get("AppInstanceIdentity");
        X509Certificate2 certificate = options.Certificate!;
        return $"""
            Certificate Subject: {certificate.Subject}, 
            Expiration: {certificate.NotAfter}
            """;
    }

    private static string _currentInstanceName = "linux";

    [HttpGet]
    public async Task<string> RotateCertificate()
    {
        _currentInstanceName = _currentInstanceName == "windows" ? "linux" : "windows";
        logger.LogInformation("Rotating to {CurrentInstanceName} instance certificate", _currentInstanceName);

        string certificatesJson = $$"""
            {
                "Certificates": {
                    "AppInstanceIdentity": {
                        "CertificateFilePath": "{{_currentInstanceName}}Instance.crt",
                        "PrivateKeyFilePath": "{{_currentInstanceName}}Instance.key"
                    }
                }
            }
            """;
        await System.IO.File.WriteAllTextAsync("certificates.json", certificatesJson);

        return $"Should now be using the instance identify certificate from a {_currentInstanceName} cell.";
    }

    [HttpGet]
    public string Anonymous()
    {
        return "This action does not require a client certificate at all";
    }

    [Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme)]
    [HttpGet]
    public string Authenticated()
    {
        return "This action requires a client certificate to be provided";
    }

    [Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme, Policy = CertificateAuthorizationPolicies.SameOrg)]
    [HttpGet]
    public string SameOrg()
    {
        logger.LogDebug("Received a request with a client certificate from the same org");
        return "Certificate is valid, client and server are in the same org.";
    }

    [Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme, Policy = CertificateAuthorizationPolicies.SameSpace)]
    [HttpGet]
    public string SameSpace()
    {
        logger.LogDebug("Received a request with a client certificate from the same space");
        return "Certificate is valid, client and server are in the same space.";
    }
}
