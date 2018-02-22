using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Redis4.Controllers
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CacheData()
        {
            var vm = new Dictionary<string, string>
            {
                { "Key1", Encoding.UTF8.GetString(_cache.Get("RedisCacheKey1")) },
                { "Key2", Encoding.UTF8.GetString(_cache.Get("RedisCacheKey2")) }
            };

            ViewBag.RedisClass = "RedisCache";

            return View("ViewData", vm);
        }

        public ActionResult ConnData()
        {
            IDatabase db = _conn.GetDatabase();

            var vm = new Dictionary<string, string>
            {
                { "Key1", db.StringGet("ConnectionMultiplexerKey1") },
                { "Key2", db.StringGet("ConnectionMultiplexerKey2") }
            };

            ViewBag.RedisClass = "ConnectionMultiplexer";

            return View("ViewData", vm);
        }
    }
}