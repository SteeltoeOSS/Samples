using MusicStoreUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreUI.Services
{
    public interface IShoppingCart
    {
        Task<List<CartItem>> GetCartItemsAsync(string cartId);
        Task<bool> RemoveItemAsync(string cartId, int itemKey);
        Task<bool> AddItemAsync(string cartId, int itemKey);
        Task<bool> EmptyCartAsync(string cartId);
        Task<bool> CreateCartAsync(string cartId);
    }
}
