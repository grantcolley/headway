using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Model;
using Headway.Core.Notifications;
using Headway.Blazor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Pages
{
    [DynamicPage]
    public abstract class PageBase : ConfigComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Parameter]
        public string Config { get; set; }

        [Parameter]
        public int? Id { get; set; }

        [Parameter]
        public string DataArgs { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (config == null)
            {
                await GetConfigAsync(Config).ConfigureAwait(false);
            }
            else if (!config.Name.Equals(Config))
            {
                config = null;

                await GetConfigAsync(Config).ConfigureAwait(false);
            }

            await base.OnParametersSetAsync().ConfigureAwait(false);

            var id = Id.HasValue ? $" {Id.Value}" : string.Empty;

            var breadcrumb = new Breadcrumb
            {
                Text = $"{config.Title}{id}",
                Href = NavigationManager.Uri.Remove(0, NavigationManager.BaseUri.Length - 1),
                ResetAfterHome = config.NavigateResetBreadcrumb
            };

            await StateNotification.NotifyStateHasChangedAsync(StateNotifications.BREADCRUMBS, breadcrumb)
                .ConfigureAwait(false);
        }

        protected RenderFragment RenderView() => builder =>
        {
            var type = Type.GetType(config.Model);
            var component = Type.GetType(config.Document);
            var genericType = component.MakeGenericType(new[] { type });
            builder.OpenComponent(1, genericType);
            builder.AddAttribute(2, Parameters.CONFIG, config.Name);

            if (Id.HasValue)
            {
                builder.AddAttribute(3, Parameters.ID, Id);
            }

            if(!string.IsNullOrWhiteSpace(DataArgs))
            {
                builder.AddAttribute(4, Parameters.DATA_ARGS, DataArgs);
            }

            builder.CloseComponent();
        };
    }
}
