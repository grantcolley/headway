using Headway.Core.Attributes;
using Headway.Core.Model;
using Headway.Blazor.Controls.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Components
{
    [DynamicComponent]
    public abstract class CheckListToTextBase : DynamicComponentBase
    {
        protected string filterString;

        protected bool FilterFunction(ChecklistItem item) => FilterItem(item, filterString);

        protected List<ChecklistItem> checklist = new();

        protected override Task OnParametersSetAsync()
        {
            LinkFieldCheck();

            var propertyValue = (string)Field.PropertyInfo.GetValue(Field.Model, null);

            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                var elements = propertyValue.Split(',');
            }

            return base.OnParametersSetAsync();
        }

        protected static void OnCheckItem(ChecklistItem item)
        {
            item.IsChecked = !item.IsChecked;
        }

        private static bool FilterItem(ChecklistItem item, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            if (item.Name != null
                && item.Name.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (item.Description != null
                && item.Description.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (item.Id.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
