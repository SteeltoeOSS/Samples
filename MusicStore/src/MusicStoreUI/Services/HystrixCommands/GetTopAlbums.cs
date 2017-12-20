using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Model = MusicStoreUI.Models;
using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreUI.Services.HystrixCommands
{
    public class GetTopAlbums : HystrixCommand<List<Model.Album>>
    {
        private const int CACHE_TIME = 30;
        private IMusicStore _storeService;
        private int _count;
        private IMemoryCache _cache;
        private AppSettings _appSettings;
        private ILogger _logger;

        private const string TOP_SELLING = "topselling";

        public GetTopAlbums(
            IHystrixCommandOptions options, 
            IMusicStore storeService, 
            IMemoryCache cache,
            IOptions<AppSettings> appsettings,
            ILogger<GetTopAlbums> logger
            ) : base(options)
        {
            _storeService = storeService;
            _cache = cache;
            _appSettings = appsettings.Value;
            _logger = logger;
            this.IsFallbackUserDefined = true;
        }

        public async Task<List<Model.Album>> GetTopSellingAlbumsAsync(int count)
        {
            _count = count;
            return await ExecuteAsync();
        }

        protected override async Task<List<Model.Album>> RunAsync()
        {
            var top = CheckCache();
            if (top != null)
            {
                _logger.LogInformation("TopAlbum returning from cache!");
                return top;
            }

            top = await _storeService.GetTopSellingAlbumsAsync(_count);

            CacheResults(top);

            _logger.LogInformation("TopAlbum returned from store!");
            return top;
        }


        protected override async Task<List<Model.Album>> RunFallbackAsync()
        {

            _logger.LogInformation("TopAlbum switching to fallback Albums!");

            var fallback = Model.SampleData.FallbackAlbums;
            List<Model.Album> results = new List<Model.Album>();
            for (int i = 0; i < _count; i++)
            {
                results.Add(fallback[i]);
            }
            return await Task.FromResult(results);

        }

        private void CacheResults(List<Model.Album> albums)
        {
            if (albums != null && albums.Count > 0)
            {
                if (_appSettings.CacheDbResults)
                {
                    _logger.LogInformation("TopAlbum caching results");

                    _cache.Set(
                        TOP_SELLING,
                        albums,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(CACHE_TIME))
                        .SetPriority(CacheItemPriority.High));
                }
            }
        }

        private List<Model.Album> CheckCache()
        {
            List<Model.Album> result = null;
            if (_appSettings.CacheDbResults)
            {
                _cache.TryGetValue(TOP_SELLING, out result);
            }
            return result;
        }
    }
}
