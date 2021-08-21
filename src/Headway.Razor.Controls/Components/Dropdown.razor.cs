using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public class DropdownBase : DynamicComponentBase
    {
        [Inject]
        public IOptionsService OptionsService { get; set; }

        protected IEnumerable<OptionItem> OptionItems;

        protected override async Task OnParametersSetAsync()
        {
            var result = await OptionsService.GetOptionItemsAsync(ComponentArgs).ConfigureAwait(false);

            OptionItems = GetResponse(result);

            await base.OnParametersSetAsync();
        }
    }
}
