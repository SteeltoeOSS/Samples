using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model =MusicStoreUI.Models;
using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStoreUI.Services.HystrixCommands
{
    public class GetGenres : HystrixCommand<List<Model.Genre>>
    {
        private const int CACHE_TIME = 30;
        private IMusicStore _storeService;
        private IMemoryCache _cache;
        private AppSettings _appSettings;
        private ILogger _logger;

        private const string GENRES_DATAMODEL = "genres";

        public GetGenres(
            IHystrixCommandOptions options,
            IMusicStore storeService,
            IMemoryCache cache,
            IOptions<AppSettings> appsettings,
            ILogger<GetGenres> logger
            ) : base(options)
        {
            _storeService = storeService;
            _cache = cache;
            _appSettings = appsettings.Value;
            _logger = logger;
            this.IsFallbackUserDefined = true;
        }

        public  async Task<List<Model.Genre>> GetGenresAsync()
        {
            return await ExecuteAsync();
        }
        protected override async Task<List<Model.Genre>> RunAsync()
        {
            var genres = CheckCache();
            if (genres != null)
            {
                _logger.LogInformation("Genres returning from cache!");
                return genres;
            }

            genres = await _storeService.GetGenresAsync();

            CacheResults(genres);

            _logger.LogInformation("Genres returned from store!");
            return genres;
        }


        protected override async Task<List<Model.Genre>> RunFallbackAsync()
        {
 
            _logger.LogInformation("Switching to fallback genres!");
            var fallbackGenres = Model.SampleData.FallbackGenres;
            foreach(var g in fallbackGenres)
            {
                _logger.LogInformation(g.GenreId, g.Name);
            }

            return await Task.FromResult(fallbackGenres);

        }

        private void CacheResults(List<Model.Genre> genres)
        {
            if (genres != null && genres.Count > 0)
            {
                if (_appSettings.CacheDbResults)
                {
                    _logger.LogInformation("Genres caching results");

                    _cache.Set(
                        GENRES_DATAMODEL,
                        genres,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(CACHE_TIME))
                        .SetPriority(CacheItemPriority.High));
                }
            }
        }

        private List<Model.Genre> CheckCache()
        {
            List<Model.Genre> result = null;
            if (_appSettings.CacheDbResults)
            {
                _cache.TryGetValue(GENRES_DATAMODEL, out result);
            }
            return result;
        }
    }
}
