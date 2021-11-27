using Headway.Core.Constants;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Pages
{
    public class PageBase : ConfigComponentBase
    {
        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public int? Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (config == null)
            {
                await GetConfig(Config).ConfigureAwait(false);
            }
            else if (!config.Name.Equals(Config))
            {
                config = null;

                await GetConfig(Config).ConfigureAwait(false);
            }

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        protected RenderFragment RenderView() => builder =>
        {
            var type = Type.GetType(config.Model);
            var component = Type.GetType(config.Container);
            var genericType = component.MakeGenericType(new[] { type });
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, Parameters.CONFIG, config.Name);

            if (Id.HasValue)
            {
                builder.AddAttribute(3, Parameters.ID, Id);
            }

            builder.CloseComponent();
        };
    }
}
