using Apache.Geode.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gemfire.Controllers
{
    public class HomeController : Controller
    {
        private readonly Random rando = new Random();
        private static IRegion<string, string> cacheRegion;
        private static Cache gemfireCache;
        private readonly List<string> sampleData = new List<string> { "Apples", "Apricots", "Avacados", "Bananas", "Blueberries", "Lemons", "Limes", "Mangos", "Oranges", "Pears", "Pineapples" };
        private static readonly string _regionName = "SteeltoeDemo";

        public HomeController(PoolFactory poolFactory, Cache cache)
        {
            Console.WriteLine("HomeController constructor");
            if (cacheRegion == null)
            {
                Console.WriteLine("Initializing stuff");
                InitializeGemFireObjects(poolFactory, cache);
            }
            Console.WriteLine("Leaving HomeController constructor");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reset()
        {
            Session.Abandon();
            Request.Cookies.Clear();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            var cacheRegion = gemfireCache.GetRegion<string, string>(_regionName);
            cacheRegion.Remove("BestFruit");
            cacheRegion = null;
            return RedirectToAction("Index");
        }

        public ActionResult GetCacheEntry()
        {
            Console.WriteLine("GetCacheEntry");
            string message;
            try
            {
                //var cacheRegion = gemfireCache.GetRegion<string, string>(_regionName);
                Console.WriteLine("Get from CacheRegion");
                message = cacheRegion["BestFruit"];
                Console.WriteLine("Got from CacheRegion {0}", message);
            }
            catch (CacheServerException)
            {
                message = "The region SteeltoeDemo has not been initialized in Gemfire.\r\nConnect to Gemfire with gfsh and run 'create region --name=SteeltoeDemo --type=PARTITION'";
            }
            catch (Apache.Geode.Client.KeyNotFoundException)
            {
                message = "Cache has not been set yet.";
            }

            ViewBag.Message = message;

            return View();
        }

        public ActionResult SetCacheEntry()
        {
            var bestfruit = sampleData.OrderBy(g => Guid.NewGuid()).First();

            //var cacheRegion = gemfireCache.GetRegion<string, string>(_regionName);
            cacheRegion["BestFruit"] = $"{bestfruit} are the best fruit. Here's random id:{rando.Next()}";

            return RedirectToAction("GetCacheEntry");
        }

        private static void InitializeGemFireObjects(PoolFactory poolFactory, Cache cache)
        {
            Console.WriteLine("Create PoolFactory");
            gemfireCache = cache;
            gemfireCache.TypeRegistry.PdxSerializer = new ReflectionBasedAutoSerializer();

            try
            {
                Console.WriteLine("Create Pool");
                // make sure the pool has been created
                poolFactory.Create("pool");
            }
            catch (IllegalStateException e)
            {
                // we end up here with this message if you've hit the reset link after the pool was created
                if (e.Message != "Pool with the same name already exists")
                {
                    throw;
                }
            }

            Console.WriteLine("Create Cache RegionFactory");
            var regionFactory = gemfireCache.CreateRegionFactory(RegionShortcut.PROXY).SetPoolName("pool");
            try
            {
                Console.WriteLine("Create CacheRegion");
                cacheRegion = regionFactory.Create<string, string>(_regionName);
                Console.WriteLine("CacheRegion created");
            }
            catch
            {
                Console.WriteLine("Create CacheRegion failed... now trying to get the region");
                cacheRegion = gemfireCache.GetRegion<string, string>(_regionName);
            }
        }
    }
}
