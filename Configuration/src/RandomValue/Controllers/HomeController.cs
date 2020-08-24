using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RandomValue.Models;

namespace RandomValue.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            ViewData["random:int"] = _config["random:int"];
            ViewData["random:long"] = _config["random:long"];
            ViewData["random:int(10)"] = _config["random:int(10)"];
            ViewData["random:long(100)"] = _config["random:long(100)"];
            ViewData["random:int(10,20)"] = _config["random:int(10,20)"];
            ViewData["random:long(100,200)"] = _config["random:long(100,200)"];
            ViewData["random:uuid"] = _config["random:uuid"];
            ViewData["random:string"] = _config["random:string"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
