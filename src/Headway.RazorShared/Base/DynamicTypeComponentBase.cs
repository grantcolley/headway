using Headway.Core.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Headway.RazorShared.Base
{
    public abstract class DynamicTypeComponentBase : HeadwayComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public async Task<string> GetTypeFullName(string typeName)
        {
            var browserStorageItem = await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", typeName).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(browserStorageItem))
            {
                var browserStorageItems = TypeAttributeHelper.GetDynamicModels();

                foreach(var storageItem in browserStorageItems)
                {
                    await JSRuntime.InvokeVoidAsync("sessionStorage.setItem", storageItem.Key, storageItem.Value);
                }

                browserStorageItem = await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", typeName).ConfigureAwait(false);
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
