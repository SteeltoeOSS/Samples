using Apache.Geode.Client;
using Gemfire.Models;
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
        private static Cache gemfireCache;
        private readonly List<string> sampleData = new List<string> { "Apples", "Apricots", "Avacados", "Bananas", "Blueberries", "Lemons", "Limes", "Mangos", "Oranges", "Pears", "Pineapples" };
        private string _regionName = "steeltoeDemo";

        public HomeController()
        {
            if (gemfireCache == null)
            {
                var cacheFactory = new CacheFactory()
                    //.Set("log-level", "fine")
                    //.Set("log-file", "C:\\steeltoe\\samples\\Connectors\\src\\AspDotNet4\\Gemfire\\pcc.log")
                    .Set("connect-timeout", "1000ms")
                    .Set("durable-timeout", "1000ms")
                    //.SetAuthInitialize(new BasicAuthInitialize("developer_zcs4XnFoWIDg14VVA7GKxA", "MGMtLoPDToFXlfnFhYZpA")); // beet
                    .SetAuthInitialize(new BasicAuthInitialize("developer_pXaEHbxknTypU6IjFFGA", "83svfPdb67k4vL1Gt09SQ")); // pcfone
                    //.SetAuthInitialize(new BasicAuthInitialize("john", "secret"));

                gemfireCache = cacheFactory.Create();

                var poolFactory = gemfireCache.GetPoolFactory()
                    //.AddLocator("10.194.45.168", 55221);
                    .AddLocator("192.168.12.220", 55221);
                    //.AddLocator("localhost", 10334);
                var pool = poolFactory.Create("pool");
                var regionFactory = gemfireCache.CreateRegionFactory(RegionShortcut.PROXY)
                    .SetPoolName("pool");
                regionFactory.Create<string, string>(_regionName);
            }
        }

        public ActionResult Index()
        {
            var items = new List<string>() { "disabled" };

            //if (Session["items"] != null)
            //{
            //    if (Session["items"] is List<string>)
            //    {
            //        items = Session["items"] as List<string>;
            //        items.Add($"Random{rando.Next()}");
            //    }
            //}

            //Session["items"] = items;

            return View(items);
        }

        public ActionResult Reset()
        {
            Session.Abandon();
            Request.Cookies.Clear();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            var cacheRegion = gemfireCache.GetRegion<string, string>(_regionName);
            cacheRegion.Remove("BestFruit");
            return RedirectToAction("Index");
        }

        public ActionResult GetCacheEntry()
        {
            string message;
            try
            {
                var cacheRegion = gemfireCache.GetRegion<string, string>(_regionName);
                message = cacheRegion.Get("BestFruit");
            }
            catch (RegionDestroyedException)
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

            var cacheRegion = gemfireCache.GetRegion<string, string>(_regionName);
            cacheRegion.Put("BestFruit", $"{bestfruit} are the best fruit.");

            return RedirectToAction("GetCacheEntry");
        }
    }
}