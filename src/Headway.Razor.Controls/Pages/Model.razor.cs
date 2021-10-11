using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Pages
{
    [DynamicPage]
    public abstract class ModelBase : ConfigComponentBase
    {
        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetConfig(Config).ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected RenderFragment RenderView() => __builder =>
        {
            var type = Type.GetType(config.Model);
            var component = Type.GetType(config.Container);
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, Parameters.CONFIG, config.Name);
            __builder.AddAttribute(3, Parameters.ID, Id);
            __builder.CloseComponent();
        };
    }
}
