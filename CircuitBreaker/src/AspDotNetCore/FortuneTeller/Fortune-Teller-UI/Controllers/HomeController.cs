using Fortune_Teller_UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        FortuneServiceCommand _fortuneServiceCommand;
        IFakeService1 _service1;
        IFortuneService _fortuneService;

        public HomeController(FortuneServiceCommand fortuneServiceCommand, IFakeService1 service1, IFortuneService fortuneService)
        {
            _fortuneServiceCommand = fortuneServiceCommand;
            _service1 = service1;
            _fortuneService = fortuneService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("random")]
        public async Task<Fortune> Random()
        {
            return await _fortuneServiceCommand.RandomFortune();
        }

        [HttpGet("random2")]
        public async Task<Fortune> RandomFromHttpClientFactory2([FromServices]IHttpClientFactory httpClientFactory)
        {
            // get with basic retry
            return await _fortuneService.RandomFortuneWithRetryAsync();
        }

        [HttpGet("random3")]
        public async Task<Fortune> RandomFromHttpClientFactory3([FromServices]IHttpClientFactory httpClientFactory)
        {
            // get with user-defined Hystrix Command
            return await _fortuneService.RandomFortuneUserCommandAsync();
        }

        [HttpGet("random4")]
        public async Task<Fortune> RandomFromHttpClientFactory4([FromServices]IHttpClientFactory httpClientFactory)
        {
            // get with default HystrixCommand
            return await _fortuneService.RandomFortuneDefaultCommandAsync();
        }

        [HttpGet("multirandom")]
        public async Task<List<Fortune>> MultiRandom()
        {
            return await _service1.RandomFortunes();
        }
    }
}
