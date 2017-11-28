using MusicStoreUI.Models;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Threading.Tasks;

namespace MusicStoreUI.Services.HystrixCommands
{
    public class AlbumCommand : HystrixCommand<Album>
    {
        private IMusicStore _storeService;
        private int? _intId;
        private string _title;

        public AlbumCommand(string GroupKeyName, IMusicStore storeService, int id) : base(HystrixCommandGroupKeyDefault.AsKey(GroupKeyName))
        {
            _storeService = storeService;
            _intId = id;
        }

        public AlbumCommand(string GroupKeyName, IMusicStore storeService, string title) : base(HystrixCommandGroupKeyDefault.AsKey(GroupKeyName))
        {
            _storeService = storeService;
            _title = title;
        }

        protected override async Task<Album> RunAsync()
        {
            if (_intId != null)
            {
                return await _storeService.GetAlbumAsync((int)_intId);
            }
            else
            {
                return await _storeService.GetAlbumAsync(_title);
            }
        }

        protected override async Task<Album> RunFallbackAsync()
        {
            return await Task.FromResult(new Album
            {
                Title = "Waiting",
                AlbumArtUrl = "https://images-na.ssl-images-amazon.com/images/I/416o2L2p1zL._AC_US110_.jpg",
                Artist = new Artist { Name = "The Backup Plan"},
                Genre = new Genre {
                    GenreId = 0,
                    Name = "Fallback",
                    Description = "The music store service is not available right now",
                }
            });
        }
    }
}
