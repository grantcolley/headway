using Headway.Core.Attributes;
using Headway.Razor.Controls.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class StringListBase : DynamicComponentBase
    {
        protected string filterString;

        protected bool FilterFunction(string item) => FilterItem(item, filterString);

        protected List<string> list = new List<string>();

        protected override Task OnParametersSetAsync()
        {
            var val = (List<string>)Field.PropertyInfo.GetValue(Field.Model, null);

            if (val != null)
            {
                list = (List<string>)Field.PropertyInfo.GetValue(Field.Model, null);
            }

            return base.OnParametersSetAsync();
        }

        private bool FilterItem(string item, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(item)
                && item.Contains(filter, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
