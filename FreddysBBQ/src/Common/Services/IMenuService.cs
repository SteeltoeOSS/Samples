using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IMenuService
    {
        Task<MenuItem> GetMenuItemAsync(long id);
        Task<List<MenuItem>> GetMenuItemsAsync();
        Task SaveMenuItemAsync(MenuItem item, bool newOne);
        Task DeleteMenuItemAsync(long id);
    }
}
