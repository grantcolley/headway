using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Enums;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Pages
{
    [DynamicPage]
    public abstract class ListBase : ConfigComponentBase
    {
        [Parameter]
        public string Config { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await GetConfig(Config).ConfigureAwait(false);

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        protected RenderFragment RenderView() => __builder =>
        {
            var type = Type.GetType(config.Model);
            var component = Type.GetType(config.Container);
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, Parameters.CONFIG, config.Name);
            __builder.CloseComponent();
        };
    }
}
