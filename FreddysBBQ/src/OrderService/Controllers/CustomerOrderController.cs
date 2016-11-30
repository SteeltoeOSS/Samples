using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.Models;
using OrderService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Common.Services;
using System.Security.Claims;
using System.Security.Principal;
using System;
using Microsoft.EntityFrameworkCore;

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
        public IEnumerable<Order> GetMyOrders()
        {
            var customerId = GetCustomerId(this.HttpContext.User.Identity);
            _logger.LogInformation("CustomerId=" + customerId);
            if (string.IsNullOrEmpty(customerId))
            {
                return new List<Order>();
            }

            return _dbContext.Orders.Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderItems);
        }

        // POST /myorders
        [HttpPost]
        [Authorize(Policy = "Orders")]
        public async Task<IActionResult> Post([FromBody]Dictionary<long, int> itemsAndQuantities)
        {

            LogClaims(this.HttpContext.User.Identity);
            Order order = new Order();
            order.CustomerId = GetCustomerId(this.HttpContext.User.Identity);
            order.Email = GetEmail(this.HttpContext.User.Identity);
            order.FirstName = GetFirstName(this.HttpContext.User.Identity);
            order.LastName = GetLastName(this.HttpContext.User.Identity);

            _logger.LogInformation("CustomerId=" + order.CustomerId);
            if (string.IsNullOrEmpty(order.CustomerId))
            {
                return BadRequest();
            }

            float total = 0;
            foreach(KeyValuePair<long,int> reqItem  in itemsAndQuantities)
            {
                long itemId = reqItem.Key;
                int quantity = reqItem.Value;
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
                    _logger.LogInformation("Unable to find menuitem: " + itemId);
                    continue;
                }
                OrderItem orderItem = new OrderItem();
                orderItem.Order = order;
                orderItem.Quantity = quantity;
                orderItem.MenuItemId = itemId;
                orderItem.Name = item.Name;
                orderItem.Price = item.Price;
                order.OrderItems.Add(orderItem);
                total = total + (item.Price * quantity);
            }
            if (order.OrderItems.Count > 0)
            {
                order.Total = total;
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
                return Ok();
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
