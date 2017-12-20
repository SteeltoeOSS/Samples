using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model = MusicStoreUI.Models;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace MusicStoreUI.Services.HystrixCommands
{
    public class GetGenre : HystrixCommand<Model.Genre>
    {
        private const int CACHE_TIME = 30;

        private IMusicStore _storeService;
        private IMemoryCache _cache;
        private AppSettings _appSettings;
        private ILogger _logger;
        private int _intId;
        private string _name;
    

        public GetGenre(
            IHystrixCommandOptions options,
            IMusicStore storeService,
            IMemoryCache cache,
            IOptions<AppSettings> appsettings,
            ILogger<GetGenre> logger
            ) : base(options)
        {
            _storeService = storeService;
            _cache = cache;
            _appSettings = appsettings.Value;
            _logger = logger;
            this.IsFallbackUserDefined = true;
        }

        public async Task<Model.Genre> GetGenreAsync(int id)
        {
            _intId = id;
            return await ExecuteAsync();
        }
        public async Task<Model.Genre> GetGenreAsync(string name)
        {
            _name = name;
            return await ExecuteAsync();
        }

        protected override async Task<Model.Genre> RunAsync()
        {
            Model.Genre result = CheckCache();

            if (result != null)
            {
                _logger.LogInformation("Genre returning from cache!");
                return result;
            }

            result = await FetchFromStoreAsync();

            CacheResult(result);

            _logger.LogInformation("Genre returned from store!");
            return result;
        }

        protected override async Task<Model.Genre> RunFallbackAsync()
        {
            var result = FetchFromFallback();
            if (result != null)
            {
                _logger.LogInformation("Genre returning from fallback cache!");
            }
            else
            {
                result = new Model.Genre()
                {
                    GenreId = _intId,
                    Name = _name,
                    Albums = new List<Model.Album>()
                };

                _logger.LogInformation("Genre returning unknown!");
            }

            return await Task.FromResult(result);
        }

        private Model.Genre FetchFromFallback()
        {
            Model.Genre result = null;

            if (string.IsNullOrEmpty(_name))
            {
                result = Model.SampleData.FallbackGenres.Where((p) => p.GenreId == _intId).SingleOrDefault();
            }
            else
            {
                result = Model.SampleData.FallbackGenres.Where((p) => p.Name == _name).SingleOrDefault();
            }

            if (result != null)
            {
                _logger.LogInformation("{0} = {1}", result.GenreId, result.Name);
            }


            return result;
        }



        private async Task<Model.Genre> FetchFromStoreAsync()
        {
            if (string.IsNullOrEmpty(_name))
            {
                return await _storeService.GetGenreAsync(_intId);
            }
            else
            {
                return await _storeService.GetGenreAsync(_name);
            }
        }

        private void CacheResult(Model.Genre genre)
        {
            if (genre != null)
            {
                if (_appSettings.CacheDbResults)
                {
                    _logger.LogInformation("Genre cached");
                    _cache.Set(
                        GetCacheKey(genre.Name),
                        genre,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(CACHE_TIME))
                        .SetPriority(CacheItemPriority.High));

                    _cache.Set(
                        GetCacheKey(genre.GenreId),
                        genre,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(CACHE_TIME))
                        .SetPriority(CacheItemPriority.High));
                }
            }
        }

        private Model.Genre CheckCache()
        {
            Model.Genre genre;
            if (string.IsNullOrEmpty(_name))
            {
                _cache.TryGetValue(GetCacheKey(_intId), out genre);
            }
            else
            {
                _cache.TryGetValue(GetCacheKey(_name), out genre);
            }
            return genre;
        }
        private string GetCacheKey<T>(T value)
        {
            return "Genre:" + value.ToString();
        }
        //private IMusicStore _storeService;
        //private int? _intId;
        //private string _stringId;

        //public GenreCommand(string groupKey, IMusicStore storeService, int id) : base(HystrixCommandGroupKeyDefault.AsKey(groupKey))
        //{
        //    _storeService = storeService;
        //    _intId = id;
        //}

        //public GenreCommand(string groupKey, IMusicStore storeService, string id) : base(HystrixCommandGroupKeyDefault.AsKey(groupKey))
        //{
        //    _storeService = storeService;
        //    _stringId = id;
        //}

        //protected override async Task<Genre> RunAsync()
        //{
        //    if (_intId != null)
        //    {
        //        return await _storeService.GetGenreAsync((int)_intId);
        //    }
        //    else
        //    {
        //        return await _storeService.GetGenreAsync(_stringId);
        //    }
        //}

        //protected override async Task<Genre> RunFallbackAsync()
        //{
        //    return await Task.FromResult(
        //        new Genre
        //        {
        //            GenreId = 0,
        //            Name = "Fallback",
        //            Description = "The music store service is not available right now",
        //            Albums = new List<Album>
        //            {
        //                new Album {
        //                    Title = "Waiting", AlbumArtUrl = "https://images-na.ssl-images-amazon.com/images/I/416o2L2p1zL._AC_US110_.jpg",
        //                    Artist = new Artist { Name = "The Backup Plan"}
        //                }
        //            }
        //        });
        //}
    }
}
