using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<List<Order>> ViewOrders()
        {
            return await _dbContext.Orders.Include(o => o.OrderItems).ToListAsync();
        }

        // POST /orders/5
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
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
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
