using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steeltoe.Security.Authorization.Certificate;

namespace Steeltoe.Samples.Server.Controllers;

[Route("api/certificate/[action]")]
[ApiController]
public class CertificateAuthorizationController(ILogger<CertificateAuthorizationController> logger) : ControllerBase
{
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

    [Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme, Policy = CertificateAuthorizationDefaults.SameOrganizationAuthorizationPolicy)]
    [HttpGet]
    public string SameOrg()
    {
        logger.LogDebug("Received a request with a client certificate from the same org");
        return "Certificate is valid, client and server are in the same org.";
    }

    [Authorize(AuthenticationSchemes = CertificateAuthenticationDefaults.AuthenticationScheme, Policy = CertificateAuthorizationDefaults.SameSpaceAuthorizationPolicy)]
    [HttpGet]
    public string SameSpace()
    {
        logger.LogDebug("Received a request with a client certificate from the same space");
        return "Certificate is valid, client and server are in the same space.";
    }
}