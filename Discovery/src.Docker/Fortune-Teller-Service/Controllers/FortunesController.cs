using FortuneTeller.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FortuneTeller.Service.Controllers
{
    [Route("api/[controller]")]
    public class FortunesController : Controller
    {
        private readonly IFortuneRepository _fortunes;
        private readonly ILogger<FortunesController> _logger;
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
