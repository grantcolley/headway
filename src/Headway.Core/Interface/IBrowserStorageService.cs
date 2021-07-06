using Headway.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IBrowserStorageService
    {
        Task<IServiceResult<IEnumerable<BrowserStorageItem>>> GetBrowserStorageItemsAsync();
    }
}
