using Headway.Core.Interface;
using Headway.Core.Mediators;
using Headway.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Base
{
    public abstract class ConfigComponentBase : HeadwayComponentBase
    {
        [Inject]
        public IMediator Mediator { get; set; }

        protected Config config;

        protected async Task GetConfig(string configName)
        {
            try
            {
                var response = await Mediator.Send(new ConfigGetByNameRequest(configName)).ConfigureAwait(false);
                config = GetResponse(response.Config);
            }
            catch (Exception ex)
            {
                RaiseAlert(ex.Message);
            }
        }
    }
}
