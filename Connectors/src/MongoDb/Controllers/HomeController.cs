using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDb.Models;
using MongoDB.Driver;
using System.Diagnostics;
using System.Linq;

namespace MongoDb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMongoCollection<Person> _mongoPeople;
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMongoClient mongoClient, MongoUrl mongoInfo, ILogger<HomeController> logger)
        {
            var db = mongoClient.GetDatabase(mongoInfo.DatabaseName ?? "TestData");
            _mongoPeople = db.GetCollection<Person>("TestDataCollection");
            _logger = logger;
        }

        public IActionResult Index()
        {
            var something = Builders<Person>.Filter.Where(p => !string.IsNullOrEmpty(p.FirstName));
            return View(_mongoPeople.Find(something).ToList());
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
