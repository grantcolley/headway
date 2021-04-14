using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IMenuRepository
    {
        Task<IEnumerable<MenuItem>> GetMenuItemsAsync();
    }
}
