using Apache.Geode.Client;
using GemFire.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GemFire.Controllers
{
    public class HomeController : Controller
    {
        private readonly Random rando = new Random();

        private static IRegion<string, string> cacheRegion;
        private static Cache gemfireCache;
        private readonly List<string> sampleData = new List<string> { "Apples", "Apricots", "Avacados", "Bananas", "Blueberries", "Lemons", "Limes", "Mangos", "Oranges", "Pears", "Pineapples" };
        private string _regionName = "SteeltoeDemo";

        public HomeController(/*PoolFactory poolFactory, Cache cache*/)
        {
            Console.WriteLine("HomeController constructor");
            if (cacheRegion == null)
            {
                Console.WriteLine("Initializing stuff");
                InitializeGemFireObjects(/*poolFactory, cache*/);
            }
            Console.WriteLine("Leaving HomeController constructor");
        }

        public IActionResult Index()
        {
            return View();
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
                Console.WriteLine("Got from CacheRegion {message}", message);
            }
            // catch (RegionDestroyedException) not sure why this isn't being thrown anymore ... ?
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

        public ActionResult Reset()
        {
            //Session.Abandon();
            //Request.Cookies.Clear();
            //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            //var cacheRegion = gemfireCache.GetRegion<string, string>(_regionName);
            cacheRegion.Remove("BestFruit");
            cacheRegion = null;
            return RedirectToAction("Index");
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

        private void InitializeGemFireObjects(/*PoolFactory poolFactory, Cache cache*/)
        {
            Console.WriteLine("Create CacheFactory");
            var cacheFactory = new CacheFactory().SetPdxIgnoreUnreadFields(true)
                .SetAuthInitialize(new BasicAuthInitialize("developer_pXaEHbxknTypU6IjFFGA", "83svfPdb67k4vL1Gt09SQ"));
            Console.WriteLine("Create Cache");
            gemfireCache = cacheFactory.Create();
            Console.WriteLine("Create PoolFactory");
            var poolFactory = gemfireCache.GetPoolFactory().AddLocator("192.168.12.220", 55221);
            //gemfireCache = cache;

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
