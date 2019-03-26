using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MusicStore.Models;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MusicStore.Controllers
{
    [Route("api/[controller]")]
    public class StoreController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;
        public StoreController(MusicStoreContext dbContext, IOptions<AppSettings> options, ILogger<StoreController> logger)
        {
            DbContext = dbContext;
            _appSettings = options.Value;
            _logger = logger;
        }

        public MusicStoreContext DbContext { get; }

        //
        // GET: /api/Store/Genres
        [HttpGet("Genres")]
        public async Task<List<GenreJson>> GetGenres()
        {
            _logger.LogDebug("GetGenres");

            var genres = await DbContext.Genres
                .Include(g => g.Albums)
                .ToListAsync();
            return GenreJson.From(genres);
        }

        //
        // GET: /api/Store/Genre?name=#
        [HttpGet("Genre")]
        public async Task<IActionResult> GetGenre(
          [FromQuery] int? id, [FromQuery] string name)
        {
             _logger.LogDebug("GetGenre");
            Genre genre = null;
            if (id.HasValue)
            {

                genre = await DbContext.Genres
                    .Include(g => g.Albums)
                    .Where(g => g.GenreId == id.Value)
                    .FirstOrDefaultAsync();
            } else
            {
                genre = await DbContext.Genres
                    .Include(g => g.Albums)
                    .Where(g => g.Name == name)
                    .FirstOrDefaultAsync();
            }


            if (genre == null)
            {
                return NotFound();
            }
            var result = GenreJson.From(genre);
            return new ObjectResult(result);
        }

        //
        // GET: /api/Store/Albums?genre=Disco
        [HttpGet("Albums")]
        public async Task<IActionResult> GetAlbums([FromQuery] string genre)
        {
            _logger.LogDebug("GetAlbums");
            // Retrieve Genre genre and its Associated associated Albums albums from database
            List<Album> albums = null;

            if ("All".Equals(genre))
            {
                albums = await DbContext.Albums
                    .Include(g => g.Artist)
                    .Include(g => g.Genre)
                    .ToListAsync();
            } else
            {
                var genreModel = await DbContext.Genres
                                .Where(g => g.Name == genre)
                                .Include(g => g.Albums)
                                .FirstOrDefaultAsync();

                if (genreModel == null)
                {
                    return NotFound();
                }

                albums = new List<Album>();
                foreach(var a in genreModel.Albums)
                {
                    var album = await DbContext.Albums
                        .Where(g => g.AlbumId == a.AlbumId)
                        .Include(g => g.Artist)
                        .Include(g => g.Genre)
                        .FirstOrDefaultAsync();
                    if (album != null)
                    {
                        albums.Add(album);
                    }
                }
       
            }

            var result = AlbumJson.From(albums);
            return new ObjectResult(result);
        }

        //
        // GET: /api/Store/Album?id=#
        // GET: /api/Store/Album?title=#
        [HttpGet("Album")]
        public async Task<IActionResult> GetAlbum(
            [FromQuery] int? id, [FromQuery] string title)
        {
            _logger.LogDebug("GetAlbum");

            Album album = null;
            if (id.HasValue)
            {
                album = await DbContext.Albums
                                .Where(a => a.AlbumId == id.Value)
                                .Include(a => a.Artist)
                                .Include(a => a.Genre)
                                .FirstOrDefaultAsync();
            } else if (!string.IsNullOrEmpty(title)) {
                album = await DbContext.Albums
                                .Include(a => a.Artist)
                                .Include(a => a.Genre)
                                .Where(a => a.Title == title).FirstOrDefaultAsync();
            }

            if (album == null)
            {
                return NotFound();
            }
            var result = AlbumJson.From(album);
            return new ObjectResult(result);
        }

        //
        // GET: /api/Store/TopSelling?count=6
        [HttpGet("TopSelling")]
        // public async Task<List<AlbumJson>> GetTopSelling([FromQuery] int count = 6)
        public List<AlbumJson> GetTopSelling([FromQuery] int count = 6)
        {
            _logger.LogDebug("GetTopSelling");
            // TODO: Current MySQL provider has a Take() bug
            // See: https://forums.mysql.com/read.php?38,650020,650020#msg-650020

            //var albumModel = await DbContext.Albums
            //    .OrderByDescending(a => a.OrderCount)
            //    .Include(a => a.Artist)
            //    .Include(a => a.Genre)
            //    .Take(count)
            //    .ToListAsync();
            //return AlbumJson.From(albumModel);

            var ordered = DbContext.Albums
                .OrderByDescending(a => a.OrderCount)
                .Include(a => a.Artist)
                .Include(a => a.Genre);

            List<Album> results = new List<Album>();
            
            foreach(var a in ordered.AsEnumerable())
            {
                results.Add(a);
                count--;
                if (count == 0)
                    break;
            }

            return AlbumJson.From(results);
        }

        //
        // GET: /api/Store/Artists
        [HttpGet("Artists")]
        public async Task<List<ArtistJson>> GetArtists()
        {
            _logger.LogDebug("GetArtists");
            var artists = await DbContext.Artists.ToListAsync();
            return ArtistJson.From(artists);
        }

        //
        // GET: /api/Store/Artist?id=#
        [HttpGet("Artist")]
        public async Task<IActionResult> GetArtist(
            [FromQuery] int id)
        {
            _logger.LogDebug("GetArtist");
            Artist artist = await DbContext.Artists
                                .Where(a => a.ArtistId == id)
                                .FirstOrDefaultAsync();
            
      

            if (artist == null)
            {
                return NotFound();
            }
            var result = ArtistJson.From(artist);
            return new ObjectResult(result);
        }


        //POST: api/Store/Album
        [HttpPost("Album/")]
        public async Task<IActionResult> AddAlbum([FromBody] AlbumJson json)
        {
            _logger.LogDebug("AddAlbum");
            if (json == null)
            {
                return BadRequest();
            }

            var toAdd = Album.From(json);

            var artist = await DbContext.Artists
                                .Where(a => a.ArtistId == toAdd.ArtistId)
                                .FirstOrDefaultAsync();

            var genre = await DbContext.Genres
                    .Where(g => g.GenreId == toAdd.GenreId)
                    .Include(g => g.Albums)
                    .FirstOrDefaultAsync();
            if (artist == null || genre == null)
            {
                return BadRequest();
            }

            toAdd.Genre = genre;
            toAdd.Genre.Albums.Add(toAdd);
            toAdd.Artist = artist;

            DbContext.Albums.Add(toAdd);
            await DbContext.SaveChangesAsync();
            return Ok();

        }

        //PUT: api/Store/Album
        [HttpPut("Album/")]
        public async Task<IActionResult> UpdateAlbum([FromBody] AlbumJson json)
        {
            _logger.LogDebug("UpdateAlbum");

            if (json == null)
            {
                return BadRequest();
            }

            var theUpdate = Album.From(json);
            var existing = await DbContext.Albums
                    .Where(a => a.AlbumId == theUpdate.AlbumId)
                    .FirstOrDefaultAsync();

            // Can't update missing album
            if (existing == null)
            {
                return NotFound();
            }


            var artist = await DbContext.Artists
                                .Where(a => a.ArtistId == theUpdate.ArtistId)
                                .FirstOrDefaultAsync();

            var genre = await DbContext.Genres
                    .Where(g => g.GenreId == theUpdate.GenreId)
                    .FirstOrDefaultAsync();

            // Cant update album if genre or artist doesnt exist
            if (artist == null || genre == null)
            {
                return BadRequest();
            }

            // Changing genre of the existing album
            if (existing.GenreId != theUpdate.GenreId)
            {
                var existingGenre = await DbContext.Genres.Where(g => g.GenreId == existing.GenreId).Include(g=> g.Albums).FirstOrDefaultAsync();
                var toRemove = existingGenre.Albums.FirstOrDefault(a => a.AlbumId == theUpdate.AlbumId);
                existingGenre.Albums.Remove(toRemove);
                DbContext.Entry(existingGenre).State = EntityState.Modified;
            }

            existing.Genre = genre;
            existing.Genre.Albums.Add(existing);
            existing.GenreId = genre.GenreId;
            existing.Artist = artist;
            existing.ArtistId = artist.ArtistId;

            existing.AlbumArtUrl = theUpdate.AlbumArtUrl;
            existing.OrderCount = theUpdate.OrderCount;
            existing.Title = theUpdate.Title;
            existing.Price = theUpdate.Price;
     
            DbContext.Entry(existing).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();
            return Ok();

        }

        //DELETE: api/Store/Album/{id}
        [HttpDelete("Album/{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            _logger.LogDebug("DeleteAlbum");

            Album album = await DbContext.Albums
                                    .Where(a => a.AlbumId == id)
                                    .FirstOrDefaultAsync();
   

            if (album == null)
            {
                return NotFound();
            }

            DbContext.Albums.Remove(album);
            await DbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
