using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Steeltoe.Common.Discovery;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Services
{
    public class OrderService : AbstractService, IOrderService
    {

        private const string ORDERS_URL = "http://order-service/orders";

        public OrderService(IDiscoveryClient client, ILoggerFactory factory, IHttpContextAccessor context) :
            base(client, factory.CreateLogger<MenuService>(), context)
        {
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var client = await GetClientAsync();
            var request = GetRequest(HttpMethod.Get, ORDERS_URL);
            return await DoRequest<List<Order>>(client, request);
        }

        public async Task RemoveOrderAsync(long id)
        {
            var client = await GetClientAsync();
            var url = ORDERS_URL + "/" + id.ToString();
            var request = GetRequest(HttpMethod.Post, url);
            await DoRequest(client, request);
        }
    }
}
