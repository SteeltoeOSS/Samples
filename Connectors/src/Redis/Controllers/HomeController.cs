using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using StackExchange.Redis;

namespace Redis.Controllers
{
    public class HomeController : Controller
    {
        private IDistributedCache _cache;
        private IConnectionMultiplexer _conn;
        public HomeController(IDistributedCache cache, IConnectionMultiplexer conn)
        {
            _cache = cache;
            _conn = conn;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult CacheData()
        {
            string key1 = Encoding.UTF8.GetString(_cache.Get("Key1"));
            string key2 = Encoding.UTF8.GetString(_cache.Get("Key2"));

            ViewData["Key1"] = key1;
            ViewData["Key2"] = key2;

            return View();
        }

        public IActionResult ConnData()
        {
            IDatabase db = _conn.GetDatabase();

            string key1 = db.StringGet("ConnectionMultiplexerKey1");
            string key2 = db.StringGet("ConnectionMultiplexerKey2");

            ViewData["ConnectionMultiplexerKey1"] = key1;
            ViewData["ConnectionMultiplexerKey2"] = key2;

            return View();
        }
    }
}
