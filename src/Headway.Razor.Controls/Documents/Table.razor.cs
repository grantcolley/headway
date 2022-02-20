using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TableBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected bool loading;
        protected string filterString;

        protected override async Task OnInitializedAsync()
        {
            loading = true;

            await InitializeDynamicListAsync().ConfigureAwait(false);

            await base.OnInitializedAsync().ConfigureAwait(false);

            loading = false;
        }

        protected void Add()
        {
            NavigationManager.NavigateTo($"{dynamicList.Config.NavigateTo}/{dynamicList.Config.NavigateToConfig}");
        }

        protected void Update(object id)
        {
            NavigationManager.NavigateTo($"{dynamicList.Config.NavigateTo}/{dynamicList.Config.NavigateToConfig}/{id}");
        }

        protected bool FilterFunction(DynamicListItem<T> item) => FilterItem(item, filterString);

        private bool FilterItem(DynamicListItem<T> item, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            foreach(var column in dynamicList.ConfigItems)
            {
                var value = dynamicList.GetValue(item.Model, column.PropertyName);

                if(value != null
                    && value.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
