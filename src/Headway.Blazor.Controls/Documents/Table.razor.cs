﻿using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using Headway.Blazor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Documents
{
    [DynamicDocument]
    public abstract class TableBase<T> : DynamicDocumentBase<T> where T : class, new()
    {
        protected string headerButtonIcon = string.Empty;
        protected string headerButtonTooltip = string.Empty;
        protected string rowButtonIcon = string.Empty;
        protected string rowButtonTooltip = string.Empty;
        protected bool showSearch = false;

        protected string filterString;

        protected bool FilterFunction(DynamicListItem<T> item) => FilterItem(item, filterString);

        protected bool HasHeaderButton { get { return !string.IsNullOrWhiteSpace(headerButtonIcon); } }

        protected bool HasRowButton { get { return !string.IsNullOrWhiteSpace(rowButtonIcon); } }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            await InitializeDynamicListAsync().ConfigureAwait(false);

            if (args.HasArg(Buttons.HEADER_BTN_IMAGE))
            {
                headerButtonIcon = args.ArgValue(Buttons.HEADER_BTN_IMAGE);
            }

            if (args.HasArg(Buttons.HEADER_BTN_TOOLTIP))
            {
                headerButtonTooltip = args.ArgValue(Buttons.HEADER_BTN_TOOLTIP);
            }

            if (args.HasArg(Buttons.ROW_BTN_IMAGE))
            {
                rowButtonIcon = args.ArgValue(Buttons.ROW_BTN_IMAGE);
            }

            if (args.HasArg(Buttons.ROW_BTN_TOOLTIP))
            {
                rowButtonTooltip = args.ArgValue(Buttons.ROW_BTN_TOOLTIP);
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
                var dataArgsJson = dynamicList.ToDataArgsJson(listItem.Model);
                NavigationManager.NavigateTo($"{dynamicList.Config.NavigatePage}/{dynamicList.Config.NavigateConfig}/{dataArgsJson}");
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
