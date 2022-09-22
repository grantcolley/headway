using Headway.Core.Constants;
using Headway.Core.Model;
using Headway.Core.Notifications;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Blazor.Shared.Common
{
    public abstract class BreadcrumbsBase : ComponentBase, IDisposable
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        protected List<BreadcrumbItem> breadcrumbs = new();

        protected string uniqueId;

        public void Dispose()
        {
            StateNotification.Deregister(StateNotifications.BREADCRUMBS);

            GC.SuppressFinalize(this);
        }

        protected override Task OnInitializedAsync()
        {
            uniqueId = Guid.NewGuid().ToString();

            StateNotification.Register(StateNotifications.BREADCRUMBS, BreadcrumbNotification);

            breadcrumbs.Add(new BreadcrumbItem("Home", href: "/", icon: Icons.Material.Filled.Home));

            return base.OnInitializedAsync();
        }

        private async Task BreadcrumbNotification(object arg)
        {
            var breadcrumb = arg as Breadcrumb;

            if (arg != null)
            {
                if (breadcrumb.ResetToHome)
                {
                    breadcrumbs.RemoveRange(1, breadcrumbs.Count - 1);
                }
                else
                {
                    if (breadcrumbs.Count > 1
                        && breadcrumb.ResetAfterHome)
                    {
                        breadcrumbs.RemoveRange(1, breadcrumbs.Count - 1);
                    }

                    var lastBreadcrumb = breadcrumbs.Last();

                    if (!lastBreadcrumb.Text.Equals(breadcrumb.Text))
                    {
                        breadcrumbs.Add(new BreadcrumbItem(breadcrumb.Text, breadcrumb.Href));
                    }
                }

                await InvokeAsync(() =>
                {
                    StateHasChanged();
                }).ConfigureAwait(true);
            }
        }
    }
}