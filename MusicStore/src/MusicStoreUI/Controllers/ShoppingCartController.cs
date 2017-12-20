using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStoreUI.Models;
using MusicStoreUI.Services;
using MusicStoreUI.Services.HystrixCommands;
using MusicStoreUI.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Command = MusicStoreUI.Services.HystrixCommands;

namespace MusicStoreUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(IShoppingCart shoppingCart, IMusicStore musicStore, ILogger<ShoppingCartController> logger)
        {
            ShoppingCartService = shoppingCart;
            MusicStoreService = musicStore;
            _logger = logger;
        }

        public IShoppingCart ShoppingCartService { get; }

        public IMusicStore MusicStoreService { get; }

        //
        // GET: /ShoppingCart/
        public async Task<IActionResult> Index()
        {
            var cart = ShoppingCart.GetCart(ShoppingCartService, MusicStoreService, null, HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = await cart.GetCartItemsAsync(),
                CartTotal = await cart.GetTotalAsync()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /ShoppingCart/AddToCart/5
        public async Task<IActionResult> AddToCart(
            [FromServices] Command.GetAlbum albumCommand,
            int id, 
            CancellationToken requestAborted)
        {
            // Retrieve the album from the database

            var addedAlbum = await albumCommand.GetAlbumAsync(id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(ShoppingCartService, MusicStoreService, null, HttpContext);

            await cart.AddToCartAsync(addedAlbum);

            _logger.LogInformation("Album {albumId} was added to the cart.", addedAlbum.AlbumId);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int id, CancellationToken requestAborted)
        {
            // Retrieve the current user's shopping cart
            var cart = ShoppingCart.GetCart(ShoppingCartService, MusicStoreService, null, HttpContext);

            // Get the name of the album to display confirmation
            var cartItems = await cart.GetCartItemsAsync();
            var cartItem = cartItems
                .Where(item => item.CartItemId == id)
                .SingleOrDefault();

            string message;
            int itemCount;
            if (cartItem != null)
            {
                // Remove from cart
                itemCount = await cart.RemoveFromCartAsync(id);

                string removed = (itemCount > 0) ? " 1 copy of " : string.Empty;
                message = removed + cartItem.Album.Title + " has been removed from your shopping cart.";
            }
            else
            {
                itemCount = 0;
                message = "Could not find this item, nothing has been removed from your shopping cart.";
            }

            // Display the confirmation message

            var results = new ShoppingCartRemoveViewModel
            {
                Message = message,
                CartTotal = await cart.GetTotalAsync(),
                CartCount = await cart.GetCountAsync(),
                ItemCount = itemCount,
                DeleteId = id
            };

            _logger.LogInformation("Album {id} was removed from a cart.", id);

            return Json(results);
        }
    }
}
