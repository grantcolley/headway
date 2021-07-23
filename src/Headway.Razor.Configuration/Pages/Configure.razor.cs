using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Razor.Components.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Razor.Configuration.Pages
{
    public partial class ConfigureBase : HeadwayComponentBase
    {
        [Inject]
        public IConfigurationService ConfigurationService { get; set; }

        protected List<ConfigType> configTypes;

        protected string selectedConfig;

        protected override async Task OnInitializedAsync()
        {
            var result = await ConfigurationService.GetConfigTypesAsync().ConfigureAwait(false);

            configTypes = new List<ConfigType>(GetResponse(result));

            configTypes.Insert(0, new ConfigType());

            base.OnInitialized();
        }

        protected void ConfigTypeSelectionChanged(ChangeEventArgs e)
        {
            selectedConfig = e.Value.ToString();
        }
    }
}
