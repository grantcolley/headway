using Headway.Core.Attributes;
using Headway.Core.Model;
using Headway.Blazor.Controls.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Components
{
    [DynamicComponent]
    public abstract class CheckListBase : DynamicComponentBase
    {
        protected string filterString;

        protected bool FilterFunction(ChecklistItem item) => FilterItem(item, filterString);

        protected List<ChecklistItem> checklist = new List<ChecklistItem>();

        protected override Task OnParametersSetAsync()
        {
            var list = (List<ChecklistItem>)Field.PropertyInfo.GetValue(Field.Model, null);

            if (list != null)
            {
                checklist = (List<ChecklistItem>)Field.PropertyInfo.GetValue(Field.Model, null);
            }

            return base.OnParametersSetAsync();
        }

        protected void OnCheckItem(ChecklistItem item)
        {
            item.IsChecked = !item.IsChecked;
        }

        private bool FilterItem(ChecklistItem item, string filter)
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
