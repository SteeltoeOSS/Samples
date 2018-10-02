
using FortuneTellerService4.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Web.Http;

namespace FortuneTellerService4.Controllers
{
    public class FortunesController : ApiController
    {
        private IFortuneRepository _fortunes;
        private ILogger<FortunesController> _logger;

        public FortunesController()
        {
            _fortunes = new FortuneRepository(SampleData.FortuneContext);
            _logger = LoggingConfig.LoggerFactory.CreateLogger<FortunesController>();
        }

        // GET: api/fortunes
        [HttpGet]
        public IEnumerable<Fortune> Get()
        {
            _logger?.LogInformation("api/fortunes");
            return _fortunes.GetAll();
        }

        // GET api/fortunes/random
        [HttpGet]
        public IHttpActionResult Random()
        {
            _logger?.LogInformation("api/fortunes/random");
            return Ok(_fortunes.RandomFortune());
        }
    }
}
