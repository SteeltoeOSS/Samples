using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;


namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(OrdersContext dbContext, ILogger<OrderController> logger)
        {
            DbContext = dbContext;
            _logger = logger;
        }

        public OrdersContext DbContext { get; }

        // POST api/Order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderJson json)
        {
            Order order = Order.From(json);

            DbContext.Orders.Add(order);

            DbContext.OrderDetails.AddRange(order.OrderDetails);

            await DbContext.SaveChangesAsync();

            return new ObjectResult(OrderJson.From(order));

        }

        [HttpGet()]
        public async Task<IActionResult> GetOrder([FromQuery] int id)
        {
            Order order = await DbContext.Orders
                                    .Where(o => o.OrderId == id)
                                    .Include(o => o.OrderDetails)
                                    .FirstOrDefaultAsync();
  
            if (order == null)
            {
                return NotFound();
            }
            var result = OrderJson.From(order);
            return new ObjectResult(result);
        }

    }
}
