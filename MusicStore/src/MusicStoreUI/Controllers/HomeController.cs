using Microsoft.AspNetCore.Mvc;
using MusicStoreUI.Services.HystrixCommands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model = MusicStoreUI.Models;

namespace MusicStoreUI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        // GET: /Home/
        public async Task<IActionResult> Index([FromServices] GetTopAlbums topAlbumsCommand)
        {
            List<Model.Album> albums = await topAlbumsCommand.GetTopSellingAlbumsAsync(6);
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