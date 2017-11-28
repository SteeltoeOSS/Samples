using MusicStoreUI.Models;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStoreUI.Services.HystrixCommands
{
    public class ArtistsAllCommand : HystrixCommand<List<Artist>>
    {
        private IMusicStore _storeService;

        public ArtistsAllCommand(string GroupKeyName, IMusicStore StoreService) : base(HystrixCommandGroupKeyDefault.AsKey(GroupKeyName))
        {
            _storeService = StoreService;
        }

        protected override async Task<List<Artist>> RunAsync()
        {
            return await _storeService.GetAllArtistsAsync();
        }

        protected override async Task<List<Artist>> RunFallbackAsync()
        {
            var fallbackResult = new List<Artist>
            {
                new Artist { Name = "The Backup Plan"}
            };

            return await Task.FromResult(fallbackResult);
        }
    }
}
