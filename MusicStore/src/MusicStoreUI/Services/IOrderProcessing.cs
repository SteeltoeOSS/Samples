using MusicStoreUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreUI.Services
{
    public interface IOrderProcessing
    {
        Task<int> AddOrderAsync(Order order);
        Task<Order> GetOrderAsync(int id);
    }
}
