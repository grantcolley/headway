using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components.Option
{
    [DynamicComponent]
    public abstract class OptionBase : DynamicComponentBase
    {
        [Inject]
        public IOptionsService OptionsService { get; set; }

        protected List<OptionItem> options;

        public string PropertyValue
        {
            get
            {
                return Field.PropertyInfo.GetValue(Field.Model)?.ToString();
            }
            set
            {
                Field.PropertyInfo.SetValue(Field.Model, value);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var arg = ComponentArgHelper.GetArg(ComponentArgs, Options.OPTIONS_CODE);

            if(arg == null)
            {
                options = ComponentArgs
                    .Select(a => new OptionItem { Id = a.Name, Display = a.Value.ToString() })
                    .ToList();
            }
            else
            {
                LinkFieldCheck();

                var result = await OptionsService.GetOptionItemsAsync(ComponentArgs).ConfigureAwait(false);
                var optionItems = GetResponse(result);
                options = new List<OptionItem>(optionItems);
            }

            await base.OnInitializedAsync().ConfigureAwait(false);
        }
    }
}
