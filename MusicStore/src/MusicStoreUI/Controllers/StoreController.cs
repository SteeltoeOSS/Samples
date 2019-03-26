using MusicStoreUI.Models;
using MusicStoreUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStoreUI.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppSettings _appSettings;

        public StoreController(IMusicStore musicStore, IOptions<AppSettings> options)
        {
            MusicStore = musicStore;
            _appSettings = options.Value;
        }

        public IMusicStore MusicStore { get; }

        //
        // GET: /Store/
        public async Task<IActionResult> Index()
        {
            var genres = await MusicStore.GetGenresAsync();

            return View(genres);
        }

        //
        // GET: /Store/Browse?genre=Disco
        public async Task<IActionResult> Browse(string genre)
        {
            
            var genreModel = await MusicStore.GetGenreAsync(genre);

            if (genreModel == null)
            {
                return NotFound();
            }

            return View(genreModel);
        }

        public async Task<IActionResult> Details(
            [FromServices] IMemoryCache cache,
            int id)
        {
            var cacheKey = string.Format("album_{0}", id);
            Album album;
            if (!cache.TryGetValue(cacheKey, out album))
            {
                album = await MusicStore.GetAlbumAsync(id);

                if (album != null)
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
