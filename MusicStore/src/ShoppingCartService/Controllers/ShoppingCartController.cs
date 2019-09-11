using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCartService.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        // GET: api/ShoppingCart/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartItems(string id)
        {

            var cart = await DbContext.Carts
                .Include(g => g.CartItems)
                .FirstOrDefaultAsync(c => c.CartId == id);

            if (cart == null)
            {
                return NotFound();
            }

            var result = CartItemJson.From(cart.CartItems);
            return new ObjectResult(result);
        }

        // PUT: api/ShoppingCart/id
        [HttpPut("{id}")]
        public async Task<IActionResult> CreateCart(string id)
        {
            var cart = await DbContext.Carts
                        .Where(c => c.CartId == id)
                        .FirstOrDefaultAsync();

            if (cart != null)
            {
                return Ok();
            }
            cart = new ShoppingCart()
            {
                CartId = id

            };
            DbContext.Carts.Add(cart);
            await DbContext.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/ShoppingCart/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(string id)
        {
            var cart = await DbContext.Carts
                        .Include(c => c.CartItems)
                        .Where(c => c.CartId == id)
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
        [HttpPut("{id}/Item/{itemId}")]
        public async Task<IActionResult> AddCartItem(string id, int itemId)
        {
            var cart = await DbContext.Carts
                       .Where(c => c.CartId == id)
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
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                cart.CartItems.Add(cartItem);

                // DbContext.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            await DbContext.SaveChangesAsync();
            return Ok();
        }

        //
        // DELETE: /api/ShoppingCart/{cartId}/Item/itemId
        [HttpDelete("{id}/Item/{itemId}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCartItem(string id, int itemId)
        {
            var cart = await DbContext.Carts
                            .Where(c => c.CartId == id)
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
