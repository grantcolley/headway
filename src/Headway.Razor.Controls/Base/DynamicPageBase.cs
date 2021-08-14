using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Base
{
    public abstract class DynamicPageBase : HeadwayComponentBase
    {
        [Inject]
        public IConfigurationService ConfigurationService { get; set; }

        protected Config config;

        protected async Task GetConfig(string configName)
        {
            try
            {
                var result = await ConfigurationService.GetConfigAsync(configName).ConfigureAwait(false);
                config = GetResponse(result);
            }
            catch (Exception ex)
            {
                RaiseAlert(ex.Message);
            }
        }
    }
}
