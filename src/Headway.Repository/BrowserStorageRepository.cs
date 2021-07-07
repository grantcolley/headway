using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class BrowserStorageRepository : RepositoryBase, IBrowserStorageRepository
    {
        public BrowserStorageRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public async Task<IEnumerable<BrowserStorageItem>> GetBrowserStorageItemsAsync()
        {
            return await applicationDbContext.BrowserStorageItems
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
