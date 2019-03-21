
using Fortune_Teller_Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fortune_Teller_Service.Controllers
{
    [Route("api/[controller]")]
    public class FortunesController : Controller
    {
        ILogger<FortunesController> _logger;

        // Lab05 Start
        private IFortuneRepository _fortunes;
        public FortunesController(ILogger<FortunesController> logger, IFortuneRepository fortunes)
        {
            _logger = logger;
            _fortunes = fortunes;
        }
        // Lab05 End


        // GET: api/fortunes/all
        [HttpGet("all")]
        // Lab10 Start
        [Authorize(Policy = "read.fortunes")]
        // Lab10 End
        public async Task<List<Fortune>> AllFortunesAsync()
        {
            _logger?.LogDebug("AllFortunesAsync");

            // Lab05 Start
            var entities = await _fortunes.GetAllAsync();
            var result = new List<Fortune>();
            foreach(var entity in entities)
            {
                result.Add(new Fortune() { Id = entity.Id, Text = entity.Text });
            }
            return result;
            // Lab05 End
        }

        // GET api/fortunes/random
        [HttpGet("random")]
        // Lab10 Start
        [Authorize(Policy = "read.fortunes")]
        // Lab10 End
        public async Task<Fortune> RandomFortuneAsync()
        {
            _logger?.LogDebug("RandomFortuneAsync");

            // Lab05 Start
            var entity = await _fortunes.RandomFortuneAsync();
            return new Fortune() { Id = entity.Id, Text = entity.Text };
            // Lab05 End
        }
    }
}
