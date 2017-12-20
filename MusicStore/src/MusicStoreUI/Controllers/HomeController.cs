using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model =MusicStoreUI.Models;
using MusicStoreUI.Services;
using MusicStoreUI.Services.HystrixCommands;

namespace MusicStoreUI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        // GET: /Home/
        public async Task<IActionResult> Index(
            [FromServices] GetTopAlbums topAlbumsCommand
            )
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