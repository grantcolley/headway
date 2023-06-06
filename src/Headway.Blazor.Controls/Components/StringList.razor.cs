using Headway.Blazor.Controls.Base;
using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Extensions;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Components
{
    [DynamicComponent]
    public abstract class StringListBase : DynamicComponentBase
    {
        protected Typo LabelTypo = Typo.inherit;

        protected string filterString;

        protected bool FilterFunction(string item) => FilterItem(item, filterString);

        protected List<string> list = new();

        protected override Task OnParametersSetAsync()
        {
            var argLabelTypo = ComponentArgs.FirstArgOrDefault(Args.LABEL_TYPO);

            if (argLabelTypo != null)
            {
                LabelTypo = (Typo)Enum.Parse(typeof(Typo), argLabelTypo.Value);
            }

            var val = (List<string>)Field.PropertyInfo.GetValue(Field.Model, null);

            if (val != null)
            {
                list = (List<string>)Field.PropertyInfo.GetValue(Field.Model, null);
            }

            return base.OnParametersSetAsync();
        }

        private static bool FilterItem(string item, string filter)
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
