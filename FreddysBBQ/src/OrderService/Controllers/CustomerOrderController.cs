using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [Route("/myorders")]
    public class CustomerOrderController : Controller
    {
        private OrderContext _dbContext;
        private ILogger<CustomerOrderController> _logger;
        private IMenuService _menuService;

        public CustomerOrderController(OrderContext dbContext, IMenuService menuService, ILoggerFactory fact)
        {
            _dbContext = dbContext;
            _logger = fact.CreateLogger<CustomerOrderController>();
            _menuService = menuService;
        }

        // GET /myorders
        [HttpGet]
        [Authorize(Policy = "Orders")]
        public async Task<List<Order>> GetMyOrders()
        {
            var customerId = GetCustomerId(this.HttpContext.User.Identity);
            _logger.LogInformation("CustomerId=" + customerId);
            if (string.IsNullOrEmpty(customerId))
            {
                return new List<Order>();
            }

            return await _dbContext.Orders.Where(o => o.CustomerId == customerId)
                        .Include(o => o.OrderItems).ToListAsync();
        }

        // POST /myorders
        [HttpPost]
        [Authorize(Policy = "Orders")]
        public async Task<IActionResult> Post([FromBody]Dictionary<long, int?> itemsAndQuantities)
        {
            if (itemsAndQuantities == null)
            {
                _logger.LogCritical("No order items detected!");
                return BadRequest("Empty orders not allowed");
            }

            LogClaims(this.HttpContext.User.Identity);
            Order order = new Order
            {
                CustomerId = GetCustomerId(this.HttpContext.User.Identity),
                Email = GetEmail(this.HttpContext.User.Identity),
                FirstName = GetFirstName(this.HttpContext.User.Identity),
                LastName = GetLastName(this.HttpContext.User.Identity)
            };

            _logger.LogInformation("CustomerId=" + order.CustomerId);
            if (string.IsNullOrEmpty(order.CustomerId))
            {
                return BadRequest();
            }

            float total = 0;
            foreach (var reqItem in itemsAndQuantities)
            {
                _logger.LogDebug("Order item key: {key}, Quantity: {quantity}", reqItem.Key, reqItem.Value ?? 0);
                long itemId = reqItem.Key;
                int quantity = reqItem.Value ?? 0;
                if (itemId < 0 || quantity < 0)
                {
                    return BadRequest();
                }

                if (quantity == 0)
                {
                    continue;
                }

                MenuItem item = await _menuService.GetMenuItemAsync(itemId);
                if (item == null)
                {
                    _logger.LogCritical("Unable to find menuitem: " + itemId);
                    continue;
                }

                OrderItem orderItem = new OrderItem
                {
                    Order = order,
                    Quantity = quantity,
                    MenuItemId = itemId,
                    Name = item.Name,
                    Price = item.Price
                };
                order.OrderItems.Add(orderItem);
                total = total + (item.Price * quantity);
            }

            if (order.OrderItems.Count > 0)
            {
                order.Total = total;
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                _logger.LogCritical("Somehow ended up with no order items");
            }

            return Ok();
        }

        private void LogClaims(IIdentity identity)
        {
            var claims = identity as ClaimsIdentity;
            if (claims == null)
            {
                _logger.LogError("Unable to access ClaimsIdentity");
                return;
            }
            foreach(Claim c in claims.Claims)
            {
                _logger.LogInformation(string.Format("Claim: {0}/{1}/{2}", c.Type, c.Value, c.ValueType));
            }
        }

        private string GetLastName(IIdentity identity)
        {
            return string.Empty;
        }

        private string GetFirstName(IIdentity identity)
        {
            return GetClaim(identity, "user_name");
        }

        private string GetEmail(IIdentity identity)
        {
            return GetClaim(identity, ClaimTypes.Email);
        }

        private string GetCustomerId(IIdentity identity)
        {
            return GetClaim(identity, "user_id");
        }

        private string GetClaim(IIdentity identity, string claim)
        {
            var claims = identity as ClaimsIdentity;
            if (claims == null)
            {
                _logger.LogError("Unable to access ClaimsIdentity");
                return null;
            }

            var idClaim = claims.FindFirst(claim);
            if (idClaim == null)
            {
                _logger.LogError("Unable to access: " + claim);
                return null;
            }

            return idClaim.Value;
        }
    }
}
