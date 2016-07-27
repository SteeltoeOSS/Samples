
using Microsoft.AspNetCore.Mvc;
using MusicStoreUI.Models;
using MusicStoreUI.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreUI.Components
{
    [ViewComponent(Name = "CartSummary")]
    public class CartSummaryComponent : ViewComponent
    {
        private IShoppingCart ShoppingCartService;
        private IMusicStore MusicStoreService;

        public CartSummaryComponent(IShoppingCart shoppingCart, IMusicStore musicStore)
        {
            ShoppingCartService = shoppingCart;
            MusicStoreService = musicStore;
        }

 

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = ShoppingCart.GetCart(ShoppingCartService, MusicStoreService, null, HttpContext);

            var cartItems = await cart.GetCartAlbumTitlesAsync();

            ViewBag.CartCount = cartItems.Count;
            ViewBag.CartSummary = string.Join("\n", cartItems.Distinct());

            return View();
        }
    }
}
