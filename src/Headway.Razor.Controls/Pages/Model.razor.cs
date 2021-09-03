using Headway.Core.Attributes;
using Headway.Core.Enums;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Pages
{
    [DynamicPage]
    public abstract class ModelBase : HeadwayComponentBase
    {
        [Inject]
        public IConfigurationService ConfigurationService { get; set; }

        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected Config config;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await ConfigurationService.GetConfigAsync(Config).ConfigureAwait(false);
                config = GetResponse(result);
            }
            catch (Exception ex)
            {
                RaiseAlert(ex.Message);
            }

            await base.OnInitializedAsync();
        }

        protected RenderFragment RenderView() => __builder =>
        {
            var type = Type.GetType(config.Model);
            var component = Type.GetType(config.Container);
            var genericType = component.MakeGenericType(new[] { type });
            __builder.OpenComponent(1, genericType);
            __builder.AddAttribute(2, "Config", config.Name);
            __builder.AddAttribute(3, "Title", config.Title);
            __builder.AddAttribute(4, "Id", Id);
            __builder.CloseComponent();
        };
    }
}
