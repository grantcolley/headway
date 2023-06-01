using Headway.Core.Attributes;
using Headway.Core.Model;
using Headway.Blazor.Controls.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Headway.Core.Interface;
using Microsoft.AspNetCore.Components;
using Headway.Core.Args;
using Headway.Core.Helpers;
using Headway.Core.Constants;
using MudBlazor;

namespace Headway.Blazor.Controls.Components
{
    [DynamicComponent]
    public abstract class CheckListToTextBase : DynamicComponentBase
    {
        [Inject]
        public IOptionsApiRequest OptionsApiRequest { get; set; }

        protected Typo LabelTypo = Typo.inherit;

        protected string filterString;

        protected bool FilterFunction(OptionCheckItem item) => FilterItem(item, filterString);

        protected List<OptionCheckItem> checklist = new();

        protected override async Task OnParametersSetAsync()
        {
            LinkFieldCheck();

            var argLabelTypo = ComponentArgHelper.GetArg(ComponentArgs, Args.LABEL_TYPO);

            if(argLabelTypo != null) 
            {
                LabelTypo = (Typo)Enum.Parse(typeof(Typo), argLabelTypo.Value);
            }

            var result = await OptionsApiRequest.GetOptionCheckItemsAsync(ComponentArgs).ConfigureAwait(false);

            checklist = new List<OptionCheckItem>(GetResponse(result));

            var propertyValue = (string)Field.PropertyInfo.GetValue(Field.Model, null);

            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                var elements = propertyValue.Split(',');
            }

            await base.OnParametersSetAsync();
        }

        protected static void OnCheckItem(OptionCheckItem item)
        {
            item.IsChecked = !item.IsChecked;
        }

        private static bool FilterItem(OptionCheckItem item, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            if (item.Id != null
                && item.Id.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (item.Display != null
                && item.Display.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
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
