using Headway.Core.Attributes;
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
                var models = TypeAttributeHelper.GetExecutingAssemblyDynamicTypesByAttribute(typeof(DynamicModelAttribute));

                foreach(var model in models)
                {
                    await JSRuntime.InvokeVoidAsync("sessionStorage.setItem", model.Name, model.Namespace);
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
