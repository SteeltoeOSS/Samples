using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Common.Models;
using OrderService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Controllers
{
    [Route("/orders")]
    [Authorize(Policy = "AdminOrders")]
    public class OrdersController : Controller
    {
        private OrderContext _dbContext { get; }
        private ILogger<OrdersController> _logger;
        public OrdersController(OrderContext dbContext, ILoggerFactory fact)
        {
            _dbContext = dbContext;
            _logger = fact.CreateLogger<OrdersController>();
        }

        // GET: /orders/
        [HttpGet]
        public IEnumerable<Order> ViewOrders()
        {
            var orders = _dbContext.Orders.ToList();
            return orders;
        }

        // POST /orders/5
        [HttpPost("{id}")]
        public IActionResult DeleteOrder(long id)
        {
            var order = _dbContext.Orders.Where(o => o.Id == id)
                .Include(o => o.OrderItems)
                .First();

            if (order == null)
            {
                return NotFound();
            }
            var items = order.OrderItems.ToList();
            foreach(var item in items)
            {
                _dbContext.OrderItems.Remove(item);
            }

            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();
            return Ok();
        }

    }
}
