using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Steeltoe.Samples.Server.Controllers;

[Route("api/JwtAuthorization")]
public class JwtAuthorizationController(IOptionsMonitor<JwtBearerOptions> jwtBearerOptionsMonitor) : Controller
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Globals.RequiredJwtScope)]
    public string Get()
    {
        return $"Jwt Bearer token is valid and includes the required scope: {Globals.RequiredJwtScope}.";
    }
}