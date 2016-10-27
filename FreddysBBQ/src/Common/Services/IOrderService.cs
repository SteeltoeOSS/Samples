using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersAsync();
        Task RemoveOrderAsync(long id);
    }
}
