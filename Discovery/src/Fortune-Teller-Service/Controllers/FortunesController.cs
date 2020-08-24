
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using FortuneTellerService.Models;
using Microsoft.Extensions.Logging;

namespace FortuneTellerService.Controllers
{
    [Route("api/[controller]")]
    public class FortunesController : Controller
    {
        private IFortuneRepository _fortunes;
        private ILogger<FortunesController> _logger;
        public FortunesController(IFortuneRepository fortunes, ILogger<FortunesController> logger)
        {
            _fortunes = fortunes;
            _logger = logger;
        }

        // GET: api/fortunes
        [HttpGet]
        public IEnumerable<Fortune> Get()
        {
            _logger?.LogInformation("GET api/fortunes");
            return _fortunes.GetAll();
        }

        // GET api/fortunes/random
        [HttpGet("random")]
        public Fortune Random()
        {
            _logger?.LogInformation("GET api/fortunes/random");
            return _fortunes.RandomFortune();
        }
    }
}
