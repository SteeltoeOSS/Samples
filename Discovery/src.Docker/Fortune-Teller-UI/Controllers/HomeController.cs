using FortuneTeller.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FortuneTeller.UI.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly IFortuneService _fortunes;

        public HomeController(IFortuneService fortunes)
        {
            _fortunes = fortunes;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _fortunes.RandomFortuneAsync();
            ViewData["fortune"] = result.Text;
            return View();
        }
    }
}
