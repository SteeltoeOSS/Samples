using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fortune_Teller_UI.Services;

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

        [HttpGet("multirandom")]
        public async Task<List<Fortune>> MultiRandom()
        {
            return await _service1.RandomFortunes();
        }
    }
}
