using FortuneTellerUI4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FortuneTellerUI4.Controllers
{
    public class HomeController : Controller
    {
        IFortuneService _fortunes;

        public HomeController(IFortuneService fortunes)
        {
            _fortunes = fortunes;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<string> Random()
        {
            return  await _fortunes.RandomFortuneAsync();
        }
    }
}