using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStoreUI.Services;
using Microsoft.AspNetCore.Http;

namespace MusicStoreUI.Models
{
    public class ShoppingCart
    {
        private readonly IShoppingCart _shoppingCart;
        private readonly IMusicStore _musicStore;
        private readonly IOrderProcessing _orderProcessing;
        private readonly string _shoppingCartId;

        private ShoppingCart(IShoppingCart shoppingCart, IMusicStore musicStore, IOrderProcessing orderProcessing, string id)
        {
            _shoppingCart = shoppingCart;
            _musicStore = musicStore;
            _orderProcessing = orderProcessing;
            _shoppingCartId = id;
        }


        public static ShoppingCart GetCart(IShoppingCart shoppingCart, IMusicStore musicStore, IOrderProcessing orderProcessing, HttpContext context)
            => GetCart(shoppingCart, musicStore, orderProcessing, GetCartIdAsync(shoppingCart, context).Result);

        public static ShoppingCart GetCart(IShoppingCart shoppingCart, IMusicStore musicStore, IOrderProcessing orderProcessing, string cartId)
            => new ShoppingCart(shoppingCart, musicStore, orderProcessing, cartId);

        public async Task AddToCartAsync(Album album)
        {
            await _shoppingCart.AddItemAsync(_shoppingCartId, album.AlbumId);
        }

        public async Task<int> RemoveFromCartAsync(int id)
        {
            // Get the cart
            var items = await _shoppingCart.GetCartItemsAsync(_shoppingCartId);

            var cartItem = items.SingleOrDefault(
                item => item.CartId == _shoppingCartId
                && item.CartItemId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    itemCount = cartItem.Count - 1;
                }
                await _shoppingCart.RemoveItemAsync(_shoppingCartId, cartItem.AlbumId);
            }

            return itemCount;
        }

        public async Task EmptyCartAsync()
        {
            await _shoppingCart.EmptyCartAsync(_shoppingCartId);
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            var items = await _shoppingCart.GetCartItemsAsync(_shoppingCartId);
            await ResolveAlbumsAsync(items);
            return items;
        }

        public async Task<List<string>> GetCartAlbumTitlesAsync()
        {
            var cartItems = await GetCartItemsAsync();

            return cartItems
                .Where(cart => cart.CartId == _shoppingCartId)
                .Select(c => c.Album.Title)
                .OrderBy(n => n)
                .ToList();
        }

        public async Task<int> GetCountAsync()
        {
            var cartItems = await GetCartItemsAsync();

            // Get the count of each item in the cart and sum them up
            return cartItems
                .Where(c => c.CartId == _shoppingCartId)
                .Select(c => c.Count)
                .Sum();
        }

        public async Task<decimal> GetTotalAsync()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            var cartItems = await GetCartItemsAsync();

            return cartItems
                .Where(c => c.CartId == _shoppingCartId)
                .Select(c => c.Album.Price * c.Count)
                .Sum();
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = await GetCartItemsAsync();
            order.OrderDetails = new List<OrderDetail>();

            List<Album> albumUpdates = new List<Album>();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var album = await _musicStore.GetAlbumAsync(item.AlbumId);
                album.OrderCount = +item.Count;
                albumUpdates.Add(album);

                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = album.Price,
                    Quantity = item.Count,
                };

                // Set the order total of the shopping cart
                orderTotal += (item.Count * album.Price);

                order.OrderDetails.Add(orderDetail);
            }

            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            //Add the Order
            var result = await _orderProcessing.AddOrderAsync(order);

            // Empty the shopping cart
            await EmptyCartAsync();

            // Update order count in albums
            foreach (var a in albumUpdates)
            {
                await _musicStore.UpdateAlbumAsync(a);
            }

            // Return the OrderId as the confirmation number
            return result;
        }

        private async Task ResolveAlbumsAsync(List<CartItem> items)
        {
            if (items == null)
            {
                return;
            }
            foreach (var item in items)
            {
                var album = await _musicStore.GetAlbumAsync(item.AlbumId);
                item.Album = album;
            }
        }

        // We're using HttpContextBase to allow access to sessions.
        private async static Task<string> GetCartIdAsync(IShoppingCart shoppingCart, HttpContext context)
        {
            var cartId = context.Session.GetString("Session");

            if (cartId == null)
            {
                //A GUID to hold the cartId. 
                cartId = Guid.NewGuid().ToString();
                context.Session.SetString("Session", cartId);
            }
            var result = await shoppingCart.CreateCartAsync(cartId);
            return cartId;
        }
    }
}