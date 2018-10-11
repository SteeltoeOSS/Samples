using Fortune_Teller_UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

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
        public async Task<IActionResult> Index()
        {
            Thread.Sleep(1000);
            var result = await _fortunes.RandomFortuneAsync();
            ViewData["fortune"] = result.Text;
            return View();
        }
    }
}
