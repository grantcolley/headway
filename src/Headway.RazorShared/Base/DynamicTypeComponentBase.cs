using Headway.Core.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Headway.RazorShared.Base
{
    public abstract class DynamicTypeComponentBase : HeadwayComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IBrowserStorageService BrowserStorageService { get; set; }

        public async Task<string> GetTypeFullName(string typeName)
        {
            var browserStorageItem = await JSRuntime.InvokeAsync<string>("localStorage.getItem", typeName).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(browserStorageItem))
            {
                var serviceResult = await BrowserStorageService.GetBrowserStorageItemsAsync().ConfigureAwait(false);

                var browserStorageItems = GetResponse(serviceResult);

                foreach(var storageItem in browserStorageItems)
                {
                    await JSRuntime.InvokeVoidAsync("localStorage.setItem", storageItem.Key, storageItem.Value);
                }

                browserStorageItem = await JSRuntime.InvokeAsync<string>("localStorage.getItem", typeName).ConfigureAwait(false);
            }

            if(string.IsNullOrWhiteSpace(browserStorageItem))
            {
                RaiseAlert($"Failed to map {typeName} to a fully qualified type name.");
                return default;
            }

            return browserStorageItem;
        }
    }
}
