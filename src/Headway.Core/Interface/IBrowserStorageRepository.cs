using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IBrowserStorageRepository : IRepository
    {
        Task<IEnumerable<BrowserStorageItem>> GetBrowserStorageItemsAsync();
    }
}
