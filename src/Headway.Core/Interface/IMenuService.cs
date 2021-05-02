using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuItem>> GetMenuItemsAsync();
    }
}
