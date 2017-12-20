using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStoreUI.Models;
using System.Net.Http;
using Steeltoe.Common.Discovery;

namespace MusicStoreUI.Services
{
    public class OrderProcessingService : BaseDiscoveryService, IOrderProcessing
    {
        private const string ORDER_URL = "http://orderprocessing/api/Order";

        public OrderProcessingService(IDiscoveryClient client, ILoggerFactory logFactory) : base(client, logFactory.CreateLogger<OrderProcessingService>())
        {
        }

        public async Task<int> AddOrderAsync(Order order)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, ORDER_URL);
            var result = await Invoke<OrderJson>(request, OrderJson.From(order));
            return result.OrderId;
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            var orderUrl = ORDER_URL + "?id=" + id;

            var request = new HttpRequestMessage(HttpMethod.Get, orderUrl);
            var orderResult = await Invoke<OrderJson>(request);

            var result = Order.From(orderResult);

            foreach(var detail in result.OrderDetails)
            {
                detail.Order = result;
            }

            return result;
        }
    }
}
