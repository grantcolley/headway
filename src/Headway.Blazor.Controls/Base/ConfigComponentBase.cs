using Headway.Core.Interface;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Base
{
    public abstract class ConfigComponentBase : HeadwayComponentBase
    {
        [Inject]
        public IConfigurationApiRequest ConfigurationApiRequest { get; set; }

        protected Config config;

        protected async Task GetConfig(string configName)
        {
            try
            {
                var response = await ConfigurationApiRequest.GetConfigAsync(configName).ConfigureAwait(false);
                config = GetResponse(response);
            }
            catch (Exception ex)
            {
                RaiseAlert(ex.Message);
            }
        }
    }
}
