using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CosmosDb
{
    public class HomeController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;

        public HomeController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var testData = await _cosmosDbService.GetTestDataAsync("SELECT * FROM c");

            return View(testData);
        }

        public async Task<IActionResult> AddData()
        {
            await _cosmosDbService.AddTestDataAsync(new TestData { Id = Guid.NewGuid().ToString(), SomeThing = Guid.NewGuid().ToString(), SomeOtherThing = Guid.NewGuid().ToString() });

            return LocalRedirect("/");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _cosmosDbService.DeleteTestDataAsync(id);

            return LocalRedirect("/");
        }
    }
}
