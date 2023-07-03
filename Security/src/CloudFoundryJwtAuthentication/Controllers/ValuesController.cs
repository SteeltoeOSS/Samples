using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CloudFoundryJwtAuthentication.Controllers;

[Route("api/[controller]")]
public class ValuesController : Controller
{
    // GET api/values
    [HttpGet]
    [Authorize/*(Policy = "testgroup")*/]
    public IEnumerable<string> Get()
    {
        return new[] { "value1", "value2" };
    }
}