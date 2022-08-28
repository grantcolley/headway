using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TableBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected string headerButtonIcon = string.Empty;
        protected string rowButtonIcon = string.Empty;
        protected bool showSearch = false;

        protected string filterString;

        protected bool FilterFunction(DynamicListItem<T> item) => FilterItem(item, filterString);

        protected bool HasHeaderButton { get { return !string.IsNullOrWhiteSpace(headerButtonIcon); } }

        protected bool HasRowButton { get { return !string.IsNullOrWhiteSpace(rowButtonIcon); } }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            await InitializeDynamicListAsync().ConfigureAwait(false);

            if (args.HasArg(Css.HEADER_BUTTON))
            {
                headerButtonIcon = args.ArgValue(Css.HEADER_BUTTON);
            }

            if (args.HasArg(Css.ROW_BUTTON))
            {
                rowButtonIcon = args.ArgValue(Css.ROW_BUTTON);
            }
        }

        protected void HeaderButtonClick()
        {
            NavigationManager.NavigateTo($"{dynamicList.Config.NavigatePage}/{dynamicList.Config.NavigateConfig}");
        }

        protected void RowButtonClick(DynamicListItem<T> listItem)
        {
            if (string.IsNullOrWhiteSpace(dynamicList.Config.NavigateProperty))
            {
                var dataArgs = dynamicList.ToDataArgsJson(listItem.Model);
                NavigationManager.NavigateTo($"{dynamicList.Config.NavigatePage}/{dynamicList.Config.NavigateConfig}/{dataArgs}");
            }
            else
            {
                var id = dynamicList.GetValue(listItem.Model, dynamicList.Config.NavigateProperty);
                NavigationManager.NavigateTo($"{dynamicList.Config.NavigatePage}/{dynamicList.Config.NavigateConfig}/{id}");
            }
        }

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
