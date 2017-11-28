using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStoreUI.Services;
using MusicStoreUI.Services.HystrixCommands;
using Steeltoe.CircuitBreaker.Hystrix;

namespace MusicStoreUI.Components
{
    [ViewComponent(Name = "GenreMenu")]
    public class GenreMenuComponent : ViewComponent
    {
        private GenresCommand _genres;

        public GenreMenuComponent(IMusicStore musicStore)
        {
            MusicStore = musicStore;
            _genres = new GenresCommand(HystrixCommandGroupKeyDefault.AsKey("MusicStoreGenres"), musicStore);
        }

        private IMusicStore MusicStore { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await _genres.ExecuteAsync();
            return View(genres.Select(g => g.Name).Take(9).ToList());
        }
    }
}