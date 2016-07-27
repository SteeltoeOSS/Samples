
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStoreUI.Services;

namespace MusicStoreUI.Components
{
    [ViewComponent(Name = "GenreMenu")]
    public class GenreMenuComponent : ViewComponent
    {
        public GenreMenuComponent(IMusicStore musicStore)
        {
            MusicStore = musicStore;
        }

        private IMusicStore MusicStore { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await MusicStore.GetGenresAsync();
            return View(genres.Select(g => g.Name).Take(9).ToList());
        }
    }
}