using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CloudFoundryJwtAuthentication.Controllers
{
    public class ValuesController : ApiController
    {
        private ILogger<ValuesController> _logger;
        public ValuesController()
        {
            _logger = ApplicationConfig.LoggerFactory.CreateLogger<ValuesController>();
        }

        // GET: api/Values
        [CustomClaimsAuthorize("testgroup")]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation("Received GET Request");
            return new string[] { "value1", "value2" };
        }
    }
}
