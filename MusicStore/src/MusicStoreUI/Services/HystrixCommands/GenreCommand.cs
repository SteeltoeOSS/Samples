using MusicStoreUI.Models;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStoreUI.Services.HystrixCommands
{
    public class GenreCommand : HystrixCommand<Genre>
    {
        private IMusicStore _storeService;
        private int? _intId;
        private string _stringId;

        public GenreCommand(string groupKey, IMusicStore storeService, int id) : base(HystrixCommandGroupKeyDefault.AsKey(groupKey))
        {
            _storeService = storeService;
            _intId = id;
        }

        public GenreCommand(string groupKey, IMusicStore storeService, string id) : base(HystrixCommandGroupKeyDefault.AsKey(groupKey))
        {
            _storeService = storeService;
            _stringId = id;
        }

        protected override async Task<Genre> RunAsync()
        {
            if (_intId != null)
            {
                return await _storeService.GetGenreAsync((int)_intId);
            }
            else
            {
                return await _storeService.GetGenreAsync(_stringId);
            }
        }

        protected override async Task<Genre> RunFallbackAsync()
        {
            return await Task.FromResult(
                new Genre
                {
                    GenreId = 0,
                    Name = "Fallback",
                    Description = "The music store service is not available right now",
                    Albums = new List<Album>
                    {
                        new Album {
                            Title = "Waiting", AlbumArtUrl = "https://images-na.ssl-images-amazon.com/images/I/416o2L2p1zL._AC_US110_.jpg",
                            Artist = new Artist { Name = "The Backup Plan"}
                        }
                    }
                });
        }
    }
}
