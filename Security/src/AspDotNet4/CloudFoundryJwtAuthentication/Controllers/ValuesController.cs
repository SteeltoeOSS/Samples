using System;
using System.Collections.Generic;
using System.Web.Http;

namespace CloudFoundryJwtAuthentication.Controllers
{
    public class ValuesController : ApiController
    {
        // GET: api/Values
        [CustomClaimsAuthorize("testgroup")]
        public IEnumerable<string> Get()
        {
            Console.WriteLine("Received GET Request");
            return new string[] { "value1", "value2" };
        }
    }
}
