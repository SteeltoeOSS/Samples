using Microsoft.Extensions.Logging;
using MusicStoreUI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicStoreUI.Services
{
    public class MusicStoreService : BaseDiscoveryService, IMusicStore
    {

        private const string TOP_SELLING_URL = "http://musicservice/api/Store/TopSelling";
        private const string GENRES_URL = "http://musicservice/api/Store/Genres";
        private const string GENRE_URL = "http://musicservice/api/Store/Genre";
        private const string ALBUMS_URL = "http://musicservice/api/Store/Albums";
        private const string ALBUM_URL = "http://musicservice/api/Store/Album";
        private const string ARTISTS_URL = "http://musicservice/api/Store/Artists";
        private const string ARTIST_URL = "http://musicservice/api/Store/Artist";

        public MusicStoreService(HttpClient client, ILoggerFactory logFactory)
            : base(client, logFactory.CreateLogger<MusicStoreService>())
        {
        }

        public async Task<Genre> GetGenreAsync(string genre)
        {
            var genreResult = await Invoke<GenreJson>(new HttpRequestMessage(HttpMethod.Get, $"{GENRE_URL}?name={genre}"));
            var result = Genre.From(genreResult);
            var albums = await Invoke<List<AlbumJson>>(new HttpRequestMessage(HttpMethod.Get, $"{ALBUMS_URL}?genre={genre}"));
            result.Albums = Album.From(albums);
            return result;
        }

        public async Task<Genre> GetGenreAsync(int id)
        {
            var genreResult = await Invoke<GenreJson>(new HttpRequestMessage(HttpMethod.Get, $"{GENRE_URL}?id={id}"));
            var result = Genre.From(genreResult);
            return result;
        }

        public async Task<List<Genre>> GetGenresAsync()
        {
            var invokeResult = await Invoke<List<GenreJson>>(new HttpRequestMessage(HttpMethod.Get, GENRES_URL));

            if (invokeResult == null)
                return new List<Genre>();

            var result = Genre.From(invokeResult);
            return result;
        }

        public async Task<List<Album>> GetTopSellingAlbumsAsync(int count = 6)
        {
            var invokeResult = await Invoke<List<AlbumJson>>(new HttpRequestMessage(HttpMethod.Get, $"{TOP_SELLING_URL}?count={count}"));
                
            if (invokeResult == null)
                return new List<Album>();

            var result = Album.From(invokeResult);
            return result;
        }

        // GET: /Store/Album?id=#
        public async Task<Album> GetAlbumAsync(int id)
        {
            var albumResult = await Invoke<AlbumJson>(new HttpRequestMessage(HttpMethod.Get, $"{ALBUM_URL}?id={id}"));
            var result = Album.From(albumResult);
            return result;
        }

        public async Task<Album> GetAlbumAsync(string title)
        {
            var albumResult = await Invoke<AlbumJson>(new HttpRequestMessage(HttpMethod.Get, $"{ALBUM_URL}?title={title}"));
            var result = Album.From(albumResult);
            return result;
        }

        public async Task<List<Album>> GetAllAlbumsAsync()
        {
            var albumResult = await Invoke<List<AlbumJson>>(new HttpRequestMessage(HttpMethod.Get, $"{ALBUMS_URL}?genre=All"));
            var result = Album.From(albumResult);
            return result;
        }

        public async Task<Artist> GetArtistAsync(int id)
        {
            var artistResult = await Invoke<ArtistJson>(new HttpRequestMessage(HttpMethod.Get, $"{ARTIST_URL}?id={id}"));
            var result = Artist.From(artistResult);
            return result;
        }

        public async Task<List<Artist>> GetAllArtistsAsync()
        {
            var invokeResult = await Invoke<List<ArtistJson>>(new HttpRequestMessage(HttpMethod.Get, ARTISTS_URL));

            if (invokeResult == null)
                return new List<Artist>();

            var result = Artist.From(invokeResult);
            return result;
        }

        public async Task<bool> AddAlbumAsync(Album album)
        {
            if (album == null)
                return false;

            var result = await Invoke(new HttpRequestMessage(HttpMethod.Post, ALBUM_URL), AlbumJson.From(album));
            return result;
        }

        public async Task<bool> UpdateAlbumAsync(Album album)
        {
            if (album == null)
                return false;

            var result = await Invoke(new HttpRequestMessage(HttpMethod.Put, ALBUM_URL), AlbumJson.From(album));
            return result;
        }

        public async Task<bool> RemoveAlbumAsync(Album album)
        {
            var result = await Invoke(new HttpRequestMessage(HttpMethod.Delete, $"{ALBUM_URL}/{album.AlbumId}"));
            return result;
        }
    }
}
