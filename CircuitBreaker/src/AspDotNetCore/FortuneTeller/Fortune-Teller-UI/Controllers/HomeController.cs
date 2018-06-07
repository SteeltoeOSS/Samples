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

        public HomeController(FortuneServiceCommand fortuneServiceCommand, IFakeService1 service1)
        {
            _fortuneServiceCommand = fortuneServiceCommand;
            _service1 = service1;
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
        public async Task<Fortune> RandomFromHttpClientFactory([FromServices]IHttpClientFactory httpClientFactory)
        {
            // use the new HttpClientFactory to get a named client pre-configured with handler pipeline 
            // see startup.cs for configuration of this pipeline
            var client = httpClientFactory.CreateClient("fortunesWithHystrixHandler");
            var httpResponse = await client.GetAsync("random");
            return await httpResponse.Content.ReadAsAsync<Fortune>();
        }

        [HttpGet("multirandom")]
        public async Task<List<Fortune>> MultiRandom()
        {
            return await _service1.RandomFortunes();
        }
    }
}
