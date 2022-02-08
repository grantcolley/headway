using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IMenuItemRepository : IRepository
    {
        Task<IEnumerable<MenuItem>> GetMenuItemsAsync();
        Task<MenuItem> GetMenuItemAsync(int id);
        Task<MenuItem> AddMenuItemAsync(MenuItem menuItem);
        Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem);
        Task<int> DeleteMenuItemAsync(int id);
    }
}