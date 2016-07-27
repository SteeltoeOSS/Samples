using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStoreUI.Models;
using Pivotal.Discovery.Client;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace MusicStoreUI.Services
{
    public class MusicStoreService :BaseDiscoveryService, IMusicStore
    {

        private const string TOP_SELLING_URL = "http://musicstore/api/Store/TopSelling";
        private const string GENRES_URL = "http://musicstore/api/Store/Genres";
        private const string GENRE_URL = "http://musicstore/api/Store/Genre";
        private const string ALBUMS_URL = "http://musicstore/api/Store/Albums";
        private const string ALBUM_URL = "http://musicstore/api/Store/Album";
        private const string ARTISTS_URL = "http://musicstore/api/Store/Artists";
        private const string ARTIST_URL = "http://musicstore/api/Store/Artist";

        public MusicStoreService(IDiscoveryClient client, ILoggerFactory logFactory) : base(client, logFactory.CreateLogger<MusicStoreService>())
        {
        }

        public async Task<Genre> GetGenreAsync(string genre)
        {
            var genreurl = GENRE_URL + "?name=" + genre;

            var request = new HttpRequestMessage(HttpMethod.Get, genreurl);
            var genreResult = await Invoke<GenreJson>(request);

            var result = Genre.From(genreResult);

            var albumsurl = ALBUMS_URL + "?genre=" + genre;
            request = new HttpRequestMessage(HttpMethod.Get, albumsurl);
            var albumResult = await Invoke<List<AlbumJson>>(request);

            result.Albums = Album.From(albumResult);
            return result;


        }

        public async Task<List<Genre>> GetGenresAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, GENRES_URL);
            var invokeResult = await Invoke<List<GenreJson>>(request);

            if (invokeResult == null)
                return new List<Genre>();

            var result = Genre.From(invokeResult);
            return result;


        }

        public async Task<List<Album>> GetTopSellingAlbumsAsync(int count = 6)
        {
            var url = TOP_SELLING_URL + "?count=" + count.ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var invokeResult = await Invoke<List<AlbumJson>>(request);
                
            if (invokeResult == null)
                return new List<Album>();

            var result = Album.From(invokeResult);
            return result;

        }

        // GET: /Store/Album?id=#
        public async Task<Album> GetAlbumAsync(int id)
        {
            var albumUrl = ALBUM_URL + "?id=" + id;

            var request = new HttpRequestMessage(HttpMethod.Get, albumUrl);
            var albumResult = await Invoke<AlbumJson>(request);

            var result = Album.From(albumResult);
            return result;
        }

        public async Task<List<Album>> GetAllAlbumsAsync()
        {
            var albumsurl = ALBUMS_URL + "?genre=All";
            var request = new HttpRequestMessage(HttpMethod.Get, albumsurl);
            var albumResult = await Invoke<List<AlbumJson>>(request);

            var result = Album.From(albumResult);
            return result;
        }

        public async Task<List<Artist>> GetAllArtistsAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, ARTISTS_URL);
            var invokeResult = await Invoke<List<ArtistJson>>(request);

            if (invokeResult == null)
                return new List<Artist>();

            var result = Artist.From(invokeResult);
            return result;
        }

        public async Task<bool> AddAlbumAsync(Album album)
        {
            if (album == null)
                return false;

            var request = new HttpRequestMessage(HttpMethod.Post, ALBUM_URL);
            var result = await Invoke(request, AlbumJson.From(album));

            return result;
         
        }

        public async Task<bool> UpdateAlbumAsync(Album album)
        {
            if (album == null)
                return false;

            var request = new HttpRequestMessage(HttpMethod.Put, ALBUM_URL);
            var result = await Invoke(request, AlbumJson.From(album));

            return result;
        }

        public async Task<bool> RemoveAlbumAsync(Album album)
        {
            var albumUrl = ALBUM_URL + "/" + album.AlbumId;

            var request = new HttpRequestMessage(HttpMethod.Delete, albumUrl);
            var result = await Invoke(request);

            return result;
        }

        public async Task<Album> GetAlbumAsync(string title)
        {
            var albumUrl = ALBUM_URL + "?title=" + title;

            var request = new HttpRequestMessage(HttpMethod.Get, albumUrl);
            var albumResult = await Invoke<AlbumJson>(request);

            var result = Album.From(albumResult);
            return result;
        }

        public async Task<Artist> GetArtistAsync(int id)
        {
            var artistUrl = ARTIST_URL + "?id=" + id;

            var request = new HttpRequestMessage(HttpMethod.Get, artistUrl);
            var artistResult = await Invoke<ArtistJson>(request);

            var result = Artist.From(artistResult);
            return result;
        }

        public async Task<Genre> GetGenreAsync(int id)
        {
            var genreurl = GENRE_URL + "?id=" + id;

            var request = new HttpRequestMessage(HttpMethod.Get, genreurl);
            var genreResult = await Invoke<GenreJson>(request);

            var result = Genre.From(genreResult);
            return result;
        }
    }
}
