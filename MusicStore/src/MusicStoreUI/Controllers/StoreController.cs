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
        private GenresCommand _genres;

        public StoreController(IMusicStore musicStore, IOptions<AppSettings> options)
        {
            MusicStore = musicStore;
            _appSettings = options.Value;
            _genres = new GenresCommand(HystrixCommandGroupKeyDefault.AsKey("MusicStoreGenres"), musicStore);
        }

        public IMusicStore MusicStore { get; }

        // GET: /Store/
        public async Task<IActionResult> Index()
        {
            var genres = await _genres.ExecuteAsync();

            return View(genres);
        }

        // GET: /Store/Browse?genre=Disco
        public async Task<IActionResult> Browse(string genre)
        {
            var genreCommand = new GenreCommand("MusicStoreGenre", MusicStore, genre);
            var genreModel = await genreCommand.ExecuteAsync();

            if (genreModel == null)
            {
                return NotFound();
            }

            return View(genreModel);
        }

        public async Task<IActionResult> Details([FromServices] IMemoryCache cache, int id)
        {
            var cacheKey = string.Format("album_{0}", id);
            if (!cache.TryGetValue(cacheKey, out Album album))
            {
                var albumCommand = new AlbumCommand("GetAlbum", MusicStore, id);
                album = await albumCommand.ExecuteAsync();

                if (album != null && !albumCommand.IsResponseFromFallback)
                {
                    if (_appSettings.CacheDbResults)
                    {
                        //Remove it from cache if not retrieved in last 10 minutes
                        cache.Set(
                            cacheKey,
                            album,
                            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                    }
                }
            }

            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }
    }
}
