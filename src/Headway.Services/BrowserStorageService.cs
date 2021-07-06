using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Services
{
    public class BrowserStorageService : ServiceBase, IBrowserStorageService
    {
        public BrowserStorageService(HttpClient httpClient)
            : base(httpClient, false)
        {
        }

        public BrowserStorageService(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, true, tokenProvider)
        {
        }

        public async Task<IServiceResult<IEnumerable<BrowserStorageItem>>> GetBrowserStorageItemsAsync()
        {
            var httpResponseMessage = await httpClient.GetAsync($"BrowserStorageItems").ConfigureAwait(false);
            return await GetServiceResult<IEnumerable<BrowserStorageItem>>(httpResponseMessage);
        }
    }
}
