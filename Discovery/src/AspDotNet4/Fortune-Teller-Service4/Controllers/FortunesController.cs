
using FortuneTellerService4.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace FortuneTellerService4.Controllers
{
    public class FortunesController : ApiController
    {
        private IFortuneRepository _fortunes;

        public FortunesController(IFortuneRepository fortunes)
        {
            _fortunes = fortunes;
        }

        // GET: api/fortunes
        [HttpGet]
        public IEnumerable<Fortune> Get()
        {
            return _fortunes.GetAll();
        }

        // GET api/fortunes/random
        [HttpGet]
        public IHttpActionResult Random()
        {
            return Ok(_fortunes.RandomFortune());
        }
    }
}
