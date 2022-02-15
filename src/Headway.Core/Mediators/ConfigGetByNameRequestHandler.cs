using Headway.Core.Interface;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Headway.Core.Mediators
{
    public class ConfigGetByNameRequestHandler : IRequestHandler<ConfigGetByNameRequest, ConfigGetByNameRequest.Response>
    {
        private readonly IConfigurationService configurationService;

        public ConfigGetByNameRequestHandler(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public async Task<ConfigGetByNameRequest.Response> Handle(ConfigGetByNameRequest request, CancellationToken cancellationToken)
        {
            return new ConfigGetByNameRequest.Response(
                await configurationService.GetConfigAsync(request.Name)
                .ConfigureAwait(false));
        }
    }
}
