using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStoreUI.Services.HystrixCommands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model = MusicStoreUI.Models;

namespace MusicStoreUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /Home/
        public async Task<IActionResult> Index([FromServices] GetTopAlbums topAlbumsCommand)
        {
            _logger.LogTrace("Loading home page");
            List<Model.Album> albums = await topAlbumsCommand.GetTopSellingAlbumsAsync(6);
            if (topAlbumsCommand.IsFailedExecution)
            {
                _logger.LogError(topAlbumsCommand.ExecutionException, "Failed to get top albums");
            }

            _logger.LogDebug("Circuit breaker execution time {responseTime}", topAlbumsCommand.ExecutionTimeInMilliseconds);

            _logger.LogInformation("Circuit breaker response recieved");
            return View(albums);
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        public IActionResult StatusCodePage()
        {
            return View("~/Views/Shared/StatusCodePage.cshtml");
        }

        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }

    }
}
