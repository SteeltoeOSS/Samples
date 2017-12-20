using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MusicStoreUI.Models;
using MusicStoreUI.Services;
using MusicStoreUI.Services.HystrixCommands;
using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Threading.Tasks;

namespace MusicStoreUI.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppSettings _appSettings;
        private GetGenres _genres;

        public StoreController(GetGenres genres, IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
            _genres = genres;
        }


        // GET: /Store/
        public async Task<IActionResult> Index()
        {
            var genres = await _genres.GetGenresAsync();

            return View(genres);
        }

        // GET: /Store/Browse?genre=Disco
        public async Task<IActionResult> Browse(
            [FromServices] Services.HystrixCommands.GetGenre genreCommand,
            string genre
            )
        {
    
            var genreModel = await genreCommand.GetGenreAsync(genre);

            if (genreModel == null)
            {
                return NotFound();
            }

            return View(genreModel);
        }

        public async Task<IActionResult> Details(
            [FromServices]  Services.HystrixCommands.GetAlbum albumCommand,
            int id)
        {
            var album = await albumCommand.GetAlbumAsync(id);
            return View(album);
        }
    }
}
