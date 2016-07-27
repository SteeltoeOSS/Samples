using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicStoreUI.Services;
using System.Threading;
using MusicStoreUI.Models;
using Microsoft.AspNetCore.Authorization;

namespace MusicStoreUI.Controllers
{

    [Authorize]
    public class CheckoutController : Controller
    {
        private const string PromoCode = "FREE";

        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(ILogger<CheckoutController> logger)
        {
            _logger = logger;
        }

        //
        // GET: /Checkout/
        public IActionResult AddressAndPayment()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddressAndPayment(
            [FromServices] IMusicStore musicStore,
            [FromServices] IOrderProcessing orders,
            [FromServices] IShoppingCart shoppingCart,
            [FromForm] Order order,
            CancellationToken requestAborted)
        {
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            var formCollection = await HttpContext.Request.ReadFormAsync();

            try
            {
                if (string.Equals(formCollection["PromoCode"].FirstOrDefault(), PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = HttpContext.User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Process the order
                    var cart = ShoppingCart.GetCart(shoppingCart, musicStore, orders, HttpContext);
                    int orderId = await cart.CreateOrderAsync(order);

                    _logger.LogInformation("User {userName} started checkout of {orderId}.", order.Username, order.OrderId);

                    return RedirectToAction("Complete", new { id = orderId });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        //
        // GET: /Checkout/Complete

        public async Task<IActionResult> Complete(
            [FromServices] IOrderProcessing orders,
            int id)
        {
            var userName = HttpContext.User.Identity.Name;

            // Validate customer owns this order
            var order = await orders.GetOrderAsync(id);
            bool isValid = order != null && order.Username.Equals(userName);

            if (isValid)
            {
                _logger.LogInformation("User {userName} completed checkout on order {orderId}.", userName, id);
                return View(id);
            }
            else
            {
                _logger.LogError(
                    "User {userName} tried to checkout with an order ({orderId}) that doesn't belong to them.",
                    userName,
                    id);
                return View("Error");
            }
        }
    }
}
