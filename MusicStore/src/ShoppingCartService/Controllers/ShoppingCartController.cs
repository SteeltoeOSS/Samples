using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCartService.Models;

using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCartService.Controllers
{
    [Route("api/[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly ILogger<ShoppingCartController> _logger;

        public ShoppingCartController(ShoppingCartContext dbContext, ILogger<ShoppingCartController> logger)
        {
            DbContext = dbContext;
            _logger = logger;
        }

        public ShoppingCartContext DbContext { get; }

        //
        // GET: api/ShoppingCart/cartId
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartItems(string cartId)
        {
    
            var cart = await DbContext.Carts
                .Where(c => c.CartId == cartId)
                .Include(g => g.CartItems)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                return NotFound();
            }

            var result = CartItemJson.From(cart.CartItems);
            return new ObjectResult(result);
        }

        // PUT: api/ShoppingCart/cartid
        [HttpPut("{cartId}")]
        public async Task<IActionResult> CreateCart(string cartId)
        {
            var cart = await DbContext.Carts
                        .Where(c => c.CartId == cartId)
                        .FirstOrDefaultAsync();

            if (cart != null)
            {
                return Ok();
            }
            cart = new ShoppingCart()
            {
                CartId = cartId

            };
            DbContext.Carts.Add(cart);
            await DbContext.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/ShoppingCart/cartid
        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCart(string cartId)
        {
            var cart = await DbContext.Carts
                        .Include(c => c.CartItems)
                        .Where(c => c.CartId == cartId)
                        .FirstOrDefaultAsync();

            if (cart == null)
            {
                return NotFound();
            }

            DbContext.CartItems.RemoveRange(cart.CartItems);
            DbContext.Carts.Remove(cart);
            await DbContext.SaveChangesAsync();
            return Ok();
        }

        //
        // PUT: api/ShoppingCart/cartid/Item/itemId
        [HttpPut("{cartId}/Item/{itemId}")]
        public async Task<IActionResult> AddCartItem(string cartId, int itemId)
        {
            var cart = await DbContext.Carts
                       .Where(c => c.CartId == cartId)
                       .Include(g => g.CartItems)
                       .FirstOrDefaultAsync();

            if (cart == null)
            {
                return NotFound();
            }

            var cartItem = cart.CartItems
                .Where(item => item.ItemKey == itemId)
                .SingleOrDefault();

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new CartItem
                {
                    ItemKey = itemId,
                    CartId = cartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                DbContext.CartItems.Add(cartItem);
            } else
            {
                cartItem.Count++;
            }
            await DbContext.SaveChangesAsync();
            return Ok();
        }

        //
        // DELETE: /api/ShoppingCart/{cartId}/Item/itemId
        [HttpDelete("{cartId}/Item/{itemId}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCartItem(string cartId, int itemId)
        {
            var cart = await DbContext.Carts
                            .Where(c => c.CartId == cartId)
                            .Include(g => g.CartItems)
                            .FirstOrDefaultAsync();

            if (cart == null)
            {
                return NotFound();
            }

            var cartItem = cart.CartItems
                .Where(item => item.ItemKey == itemId)
                .SingleOrDefault();

            if (cartItem == null)
            {
                return NotFound();
            }


            if (cartItem.Count > 1)
            {
                cartItem.Count--;
            }
            else
            {
                cartItem.Count--;
                DbContext.CartItems.Remove(cartItem);
            }

            await DbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
