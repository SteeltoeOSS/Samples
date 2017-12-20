using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicStoreUI.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Steeltoe.Common.Discovery;

namespace MusicStoreUI.Services
{
    public class ShoppingCartService : BaseDiscoveryService, IShoppingCart
    {
        private const string SHOPPINGCART_URL = "http://shoppingcart/api/ShoppingCart/{cartId}";
        private const string SHOPPINGCART_ITEM_URL ="http://shoppingcart/api/ShoppingCart/{cartId}/Item/{itemId}";

        public ShoppingCartService(IDiscoveryClient client, ILoggerFactory logFactory) :
            base(client, logFactory.CreateLogger<ShoppingCartService>())
        {
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
            var cartResult = await Invoke<List<CartItemJson>>(request);

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
            var result = await Invoke(request);
            return result;
        }
    }
}
