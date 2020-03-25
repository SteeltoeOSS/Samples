using Microsoft.Extensions.Logging;
using MusicStoreUI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicStoreUI.Services
{
    public class ShoppingCartService : BaseDiscoveryService, IShoppingCart
    {
        private const string SHOPPINGCART_URL = "http://shoppingcartservice/api/ShoppingCart/{cartId}";
        private const string SHOPPINGCART_ITEM_URL ="http://shoppingcartservice/api/ShoppingCart/{cartId}/Item/{itemId}";
        private new ILogger _logger;

        public ShoppingCartService(HttpClient client, ILoggerFactory logFactory) :
            base(client, logFactory.CreateLogger<ShoppingCartService>())
        {
            _logger = logFactory.CreateLogger<ShoppingCartService>();
        }

        public async Task<bool> EmptyCartAsync(string cartId)
        {
            var cartUrl = SHOPPINGCART_URL.Replace("{cartId}", cartId);

            var request = new HttpRequestMessage(HttpMethod.Delete, cartUrl);
            var result = await Invoke(request);
            return result;
        }

        public async Task<List<CartItem>> GetCartItemsAsync(string cartId)
        {
            var cartUrl = SHOPPINGCART_URL.Replace("{cartId}", cartId);

            var request = new HttpRequestMessage(HttpMethod.Get, cartUrl);
            var cartResult = new List<CartItemJson>();
            try
            {
                cartResult = await Invoke<List<CartItemJson>>(request);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to get cart!");
            }

            var result = CartItem.From(cartResult);
            return result;
        }

        public async Task<bool> RemoveItemAsync(string cartId, int itemKey)
        {
            var cartUrl = SHOPPINGCART_ITEM_URL.Replace("{cartId}", cartId).Replace("{itemId}", itemKey.ToString());

            var request = new HttpRequestMessage(HttpMethod.Delete, cartUrl);
            var result = await Invoke(request);
            return result;
        }

        public async Task<bool> AddItemAsync(string cartId, int itemKey)
        {
            var cartUrl = SHOPPINGCART_ITEM_URL.Replace("{cartId}", cartId).Replace("{itemId}", itemKey.ToString());

            var request = new HttpRequestMessage(HttpMethod.Put, cartUrl);
            var result = await Invoke(request);
            return result;
        }

        public async Task<bool> CreateCartAsync(string cartId)
        {
            var cartUrl = SHOPPINGCART_URL.Replace("{cartId}", cartId);

            var request = new HttpRequestMessage(HttpMethod.Put, cartUrl);
            try
            {
                var result = await Invoke(request);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to create cart!");
                return false;
            }
        }
    }
}
