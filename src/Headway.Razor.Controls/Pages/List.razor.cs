using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Pages
{
    [DynamicPage(PageType.List)]
    public abstract class ListBase : DynamicPageBase
    {
        [Parameter]
        public string Config { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await GetConfig(Config);

            await base.OnParametersSetAsync();
        }

        protected RenderFragment RenderView() => __builder =>
        {
            var type = Type.GetType(config.Model);
            var component = Type.GetType(config.Container);
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "Config", config.Name);
            __builder.CloseComponent();
        };
    }
}
