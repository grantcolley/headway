using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Razor.Components.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Components.Pages
{
    [DynamicContainer(ContainerType.List)]
    public abstract class ListBase : DynamicTypeComponentBase
    {
        [Parameter]
        public string Config { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await GetConfig(Config);

            await base.OnParametersSetAsync();
        }

        protected RenderFragment RenderListView() => __builder =>
        {
            var type = Type.GetType(modelNameSpace);
            var component = Type.GetType(componentNameSpace);
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "Config", config.Name);
            __builder.CloseComponent();
        };
    }
}
