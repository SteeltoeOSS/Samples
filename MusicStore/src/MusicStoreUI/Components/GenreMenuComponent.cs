using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStoreUI.Services;
using MusicStoreUI.Services.HystrixCommands;
using Models = MusicStoreUI.Models;
using System.Collections.Generic;

namespace MusicStoreUI.Components
{
    [ViewComponent(Name = "GenreMenu")]
    public class GenreMenuComponent : ViewComponent
    {
        private GetGenres _genres;

        public GenreMenuComponent(GetGenres genres)
        {
            _genres = genres;
        }

        private IMusicStore MusicStore { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
             List<Models.Genre> genres = await _genres.GetGenresAsync();
            return View(genres.Select(g => g.Name).Take(9).ToList());
        }
    }
}