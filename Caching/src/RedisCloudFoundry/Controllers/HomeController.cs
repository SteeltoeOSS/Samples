using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pivotal.Extensions.Caching;
using System.Text;
using Pivotal.Extensions.Caching.Distributed;

namespace RedisCloudFoundry.Controllers
{
    public class HomeController : Controller
    {
        private IRedisDistributedCache _cache;
        public HomeController(IRedisDistributedCache cache)
        {
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult CacheData()
        {
            string key1 = Encoding.Default.GetString(_cache.Get("Key1"));
            string key2 = Encoding.Default.GetString(_cache.Get("Key2"));

            ViewData["Key1"] = key1;
            ViewData["Key2"] = key2;

            return View();
        }
    }
}
