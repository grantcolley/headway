using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Pages
{
    [DynamicContainer(ContainerType.Model)]
    public abstract class ModelBase : DynamicTypeComponentBase
    {
        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetConfig(Config);

            await base.OnInitializedAsync();
        }

        protected RenderFragment RenderListView() => __builder =>
        {
            var type = Type.GetType(modelNameSpace);
            var component = Type.GetType(componentNameSpace);
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "Config", config.Name);
            __builder.AddAttribute(3, "Title", config.Title);
            __builder.AddAttribute(4, "Id", Id);
            __builder.CloseComponent();
        };
    }
}
