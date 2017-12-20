using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model = MusicStoreUI.Models;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MusicStoreUI.Services.HystrixCommands
{
    public class GetAlbum : HystrixCommand<Model.Album>
    {
        private const int CACHE_TIME = 30;

        private IMusicStore _storeService;
        private IMemoryCache _cache;
        private AppSettings _appSettings;
        private ILogger _logger;
        private int _intId;
        private string _name;


        public GetAlbum(
            IHystrixCommandOptions options,
            IMusicStore storeService,
            IMemoryCache cache,
            IOptions<AppSettings> appsettings,
            ILogger<GetAlbum> logger
            ) : base(options)
        {
            _storeService = storeService;
            _cache = cache;
            _appSettings = appsettings.Value;
            _logger = logger;
            this.IsFallbackUserDefined = true;
        }

        public async Task<Model.Album> GetAlbumAsync(int id)
        {
            _intId = id;
            return await ExecuteAsync();
        }
        public async Task<Model.Album> GetAlbumAsync(string name)
        {
            _name = name;
            return await ExecuteAsync();
        }

        protected override async Task<Model.Album> RunAsync()
        {
            Model.Album result = CheckCache();
            if (result != null)
            {
                _logger.LogInformation("Album returning from cache!");
                return result;
            }

            result = await FetchFromStoreAsync();

            CacheResult(result);

            _logger.LogInformation("Album returned from store!");
            return result;
        }


        protected override async Task<Model.Album> RunFallbackAsync()
        {
            _logger.LogInformation("Album trying fallback!");

            var result = FetchFromFallback();
            if (result != null)
            {
                _logger.LogInformation("Album returning from fallback cache!");
            } else
            {

                result = new Model.Album()
                {
                    AlbumArtUrl = Model.SampleData.ImageUrl,
                    Genre = new Model.Genre()
                    {
                        Name = "Not Available"
                     },
                    AlbumId = 0,
                    Title = _name ?? "Not Available",
                    Artist = new Model.Artist()
                    {
                        Name = "Not Available",
                        ArtistId = 0
                    },
                    Price = 0
                };

                _logger.LogInformation("Album returning unknown!");
            }

            return await Task.FromResult(result);
        }

        private Model.Album FetchFromFallback()
        {
            _logger.LogInformation("Album fetching from fallback cache!");
            Model.Album result;

            if (string.IsNullOrEmpty(_name))
            {
                result = Model.SampleData.FallbackAlbums.Where((p) => p.AlbumId == _intId).SingleOrDefault();
            }
            else
            {
                result = Model.SampleData.FallbackAlbums.Where((p) => p.Title == _name).SingleOrDefault();
            }
            _logger.LogInformation("Back");

            if (result != null)
            {
                _logger.LogInformation("{0} = {1}", result.AlbumId, result.Title);
            }

            return result;
        }

        private async Task<Model.Album> FetchFromStoreAsync()
        {
            if (string.IsNullOrEmpty(_name))
            {
                return await _storeService.GetAlbumAsync(_intId);
            }
            else
            {
                return await _storeService.GetAlbumAsync(_name);
            }
        }

        private void CacheResult(Model.Album album)
        {
            if (album != null)
            {
                if (_appSettings.CacheDbResults)
                {
                    _logger.LogInformation("Album cached");
                    _cache.Set(
                        GetCacheKey(album.Title),
                        album,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(CACHE_TIME))
                        .SetPriority(CacheItemPriority.High));

                    _cache.Set(
                        GetCacheKey(album.AlbumId),
                        album,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(CACHE_TIME))
                        .SetPriority(CacheItemPriority.High));
                }
            }
        }

        private Model.Album CheckCache()
        {
            Model.Album album;
            if (string.IsNullOrEmpty(_name))
            {
                _cache.TryGetValue(GetCacheKey(_intId), out album);
            }
            else
            {
                _cache.TryGetValue(GetCacheKey(_name), out album);
            }
            return album;
        }
        private string GetCacheKey<T>(T value)
        {
            return "Album:" + value.ToString();
        }
    }
  
}
