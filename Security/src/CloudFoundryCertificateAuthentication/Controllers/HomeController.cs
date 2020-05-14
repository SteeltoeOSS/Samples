using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Steeltoe.Security.Authentication.CloudFoundry;

namespace ServerApp.Controllers
{
    [Route("api")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(CloudFoundryDefaults.SameOrganizationAuthorizationPolicy)]
        [HttpGet("[action]")]
        public string SameOrgCheck()
        {
            _logger.LogDebug("Received a request with a client certificate from the same org");
            return "Certificate is valid and both client and server are in the same org";
        }

        [Authorize(CloudFoundryDefaults.SameSpaceAuthorizationPolicy)]
        [HttpGet("[action]")]
        public string SameSpaceCheck()
        {
            _logger.LogDebug("Received a request with a client certificate from the same space");
            return "Certificate is valid and both client and server are in the same space";
        }
    }
}
