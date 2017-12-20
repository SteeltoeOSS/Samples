using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MusicStoreUI.Models;
using MusicStoreUI.ViewModels;
using MusicStoreUI.Services;
using System.Collections.Generic;

namespace MusicStoreUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("ManageStore")]
    public class StoreManagerController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IMusicStore MusicStoreService;

        public StoreManagerController(IMusicStore musicStore, IOptions<AppSettings> options)
        {
            MusicStoreService = musicStore;
            _appSettings = options.Value;
        }


        //
        // GET: /StoreManager/
        public async Task<IActionResult> Index()
        {
            try
            {
                var albums = await MusicStoreService.GetAllAlbumsAsync();
                return View(albums);
            } catch 
            {
                return View(new List<Album>());
            }
        }

        //
        // GET: /StoreManager/Details/5
        public async Task<IActionResult> Details(
            [FromServices] IMemoryCache cache,
            int id)
        {
            var cacheKey = GetCacheKey(id);

            Album album;
            if (!cache.TryGetValue(cacheKey, out album))
            {
                album = await MusicStoreService.GetAlbumAsync(id);

                if (album != null)
                {
                    if (_appSettings.CacheDbResults)
                    {
                        //Remove it from cache if not retrieved in last 10 minutes.
                        cache.Set(
                            cacheKey,
                            album,
                            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(10)));
                    }
                }
            }

            if (album == null)
            {
                cache.Remove(cacheKey);
                return NotFound();
            }

            return View(album);
        }

        //
        // GET: /StoreManager/Create
        public async Task<IActionResult> Create()
        {
            var genres = await MusicStoreService.GetGenresAsync();
            var artists = await MusicStoreService.GetAllArtistsAsync();

            ViewBag.GenreId = new SelectList(genres, "GenreId", "Name");
            ViewBag.ArtistId = new SelectList(artists, "ArtistId", "Name");
            return View();
        }

        // POST: /StoreManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Album album,
            [FromServices] IMemoryCache cache,
            CancellationToken requestAborted)
        {
            if (ModelState.IsValid)
            {
                album.Artist = await MusicStoreService.GetArtistAsync(album.ArtistId);
                album.Genre = await MusicStoreService.GetGenreAsync(album.GenreId);

                await MusicStoreService.AddAlbumAsync(album);


                var albumData = new AlbumData
                {
                    Title = album.Title,
                    Url = Url.Action("Details", "Store", new { id = album.AlbumId })
                };
                if (cache != null)
                {
                    cache.Remove("latestAlbum");
                }

                return RedirectToAction("Index");
            }

            var genres = await MusicStoreService.GetGenresAsync();
            var artists = await MusicStoreService.GetAllArtistsAsync();

            ViewBag.GenreId = new SelectList(genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // GET: /StoreManager/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var album = await MusicStoreService.GetAlbumAsync(id);

            if (album == null)
            {
                return NotFound();
            }
            var genres = await MusicStoreService.GetGenresAsync();
            var artists = await MusicStoreService.GetAllArtistsAsync();

            ViewBag.GenreId = new SelectList(genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // POST: /StoreManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [FromServices] IMemoryCache cache,
            Album album,
            CancellationToken requestAborted)
        {
            if (ModelState.IsValid)
            {
                album.Artist = await MusicStoreService.GetArtistAsync(album.ArtistId);
                album.Genre = await MusicStoreService.GetGenreAsync(album.GenreId);
                await MusicStoreService.UpdateAlbumAsync(album);
                //Invalidate the cache entry as it is modified
                cache.Remove(GetCacheKey(album.AlbumId));
                return RedirectToAction("Index");
            }

            var genres = await MusicStoreService.GetGenresAsync();
            var artists = await MusicStoreService.GetAllArtistsAsync();

            ViewBag.GenreId = new SelectList(genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // GET: /StoreManager/RemoveAlbum/5
        public async Task<IActionResult> RemoveAlbum(int id)
        {
            var album = await MusicStoreService.GetAlbumAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        //
        // POST: /StoreManager/RemoveAlbum/5
        [HttpPost, ActionName("RemoveAlbum")]
        public async Task<IActionResult> RemoveAlbumConfirmed(
            [FromServices] IMemoryCache cache,
            int id,
            CancellationToken requestAborted)
        {
            var album = await MusicStoreService.GetAlbumAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            await MusicStoreService.RemoveAlbumAsync(album);

            //Remove the cache entry as it is removed
            cache.Remove(GetCacheKey(id));

            return RedirectToAction("Index");
        }

        private static string GetCacheKey(int id)
        {
            return string.Format("album_{0}", id);
        }

#if TESTING
        //
        // GET: /StoreManager/GetAlbumIdFromName
        // Note: Added for automated testing purpose. Application does not use this.
        [HttpGet]
        [SkipStatusCodePages]
        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> GetAlbumIdFromName(string albumName)
        {
            var album = await MusicStoreService.GetAlbumAsync(albumName);

            if (album == null)
            {
                return NotFound();
            }

            return Content(album.AlbumId.ToString());
        }
#endif
    }
}