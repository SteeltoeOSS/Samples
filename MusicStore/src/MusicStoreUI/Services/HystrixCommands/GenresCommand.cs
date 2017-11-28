using Microsoft.Extensions.Logging;
using MusicStoreUI.Models;
using Steeltoe.CircuitBreaker.Hystrix;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStoreUI.Services.HystrixCommands
{
    public class GenresCommand : HystrixCommand<List<Genre>>
    {
        private IMusicStore _storeService;

        public GenresCommand(IHystrixCommandGroupKey groupKey, IMusicStore storeService) : base(groupKey)
        {
            _storeService = storeService;
        }

        protected override async Task<List<Genre>> RunAsync()
        {
            return await _storeService.GetGenresAsync();
        }

        protected override async Task<List<Genre>> RunFallbackAsync()
        {
            List<Genre> results = new List<Genre>
            {
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
                }
            };
            return await Task.FromResult(results);
        }
    }
}
