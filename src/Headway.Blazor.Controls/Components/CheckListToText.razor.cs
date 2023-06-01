using Headway.Blazor.Controls.Base;
using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                static OptionCheckItem CheckItem(OptionCheckItem o)
                {
                    o.IsChecked = true;
                    return o;
                }

                var elements = propertyValue.Split(',');

                foreach( var element in elements)
                {
                    _ = (from c in checklist
                            join e in elements on c.Id equals e
                            select CheckItem(c)).ToList();
                }
            }

            await base.OnParametersSetAsync();
        }

        protected void OnCheckItem(OptionCheckItem item)
        {
            item.IsChecked = !item.IsChecked;

            var values = string.Join(",", checklist.Where(o => o.IsChecked).Select(o => o.Display));

            if (string.IsNullOrWhiteSpace(values))
            {
                Field.PropertyInfo.SetValue(Field.Model, null);
            }
            else
            {
                Field.PropertyInfo.SetValue(Field.Model, values);
            }
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
