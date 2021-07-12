using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.RazorShared.Pages
{
    public partial class ConfigureBase : ComponentBase
    {
        protected IEnumerable<DynamicType> models;
        protected IEnumerable<DynamicType> containers;

        protected override async Task OnInitializedAsync()
        {
            models = TypeAttributeHelper.GetExecutingAssemblyDynamicTypesByAttribute(typeof(DynamicModelAttribute));
            containers = TypeAttributeHelper.GetCallingAssemblyDynamicTypesByAttribute(typeof(DynamicContainerAttribute));

            await base.OnInitializedAsync();
        }
    }
}
