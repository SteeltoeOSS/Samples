using Fortune_Teller_UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        IFortuneService _fortunes;

        public HomeController(IFortuneService fortunes)
        {
            _fortunes = fortunes;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("random")]
        public async Task<string> Random()
        {
            return await _fortunes.RandomFortuneAsync();
        }
    }
}
