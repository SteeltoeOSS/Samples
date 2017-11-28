using MusicStoreUI.Models;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStoreUI.Services.HystrixCommands
{
    public class AlbumsTopCommand : HystrixCommand<List<Album>>
    {
        private IMusicStore _storeService;
        private int _count;

        public AlbumsTopCommand(string GroupKeyName, IMusicStore StoreService, int Count) : base(HystrixCommandGroupKeyDefault.AsKey(GroupKeyName))
        {
            _count = Count;
            _storeService = StoreService;
        }

        protected override async Task<List<Album>> RunAsync()
        {
            return await _storeService.GetTopSellingAlbumsAsync(_count);
        }

        protected override async Task<List<Album>> RunFallbackAsync()
        {
            var fallbackResult = new List<Album>
            {
                new Album { Title = "Waiting", AlbumArtUrl = "https://images-na.ssl-images-amazon.com/images/I/416o2L2p1zL._AC_US110_.jpg", Artist = new Artist { Name = "The Backup Plan"} }
            };

            return await Task.FromResult(fallbackResult);
        }
    }
}
