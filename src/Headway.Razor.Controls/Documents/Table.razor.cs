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
        protected string headerButtonIcon = Css.BTN_IMAGE_PLUS;
        protected string rowButtonIcon = Css.BTN_IMAGE_EDIT;
        protected bool showSearch = false;

        protected string filterString;

        protected bool FilterFunction(DynamicListItem<T> item) => FilterItem(item, filterString);

        protected override async Task OnInitializedAsync()
        {
            await InitializeDynamicListAsync().ConfigureAwait(false);

            if (args.HasArg(Css.HEADER_BUTTON))
            {
                headerButtonIcon = args.ArgValue(Css.HEADER_BUTTON);
            }

            if (args.HasArg(Css.ROW_BUTTON))
            {
                rowButtonIcon = args.ArgValue(Css.ROW_BUTTON);
            }

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected void Add()
        {
            NavigationManager.NavigateTo($"{dynamicList.Config.NavigatePage}/{dynamicList.Config.NavigateConfig}");
        }

        protected void Update(object id)
        {
            NavigationManager.NavigateTo($"{dynamicList.Config.NavigatePage}/{dynamicList.Config.NavigateConfig}/{id}");
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
