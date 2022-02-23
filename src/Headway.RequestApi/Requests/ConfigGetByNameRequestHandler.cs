using Headway.Core.Interface;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Headway.RequestApi.Requests
{
    public class ConfigGetByNameRequestHandler : IRequestHandler<ConfigGetByNameRequest, ConfigGetByNameRequest.Response>
    {
        private readonly IConfigurationApiRequest configurationApiRequest;

        public ConfigGetByNameRequestHandler(IConfigurationApiRequest configurationApiRequest)
        {
            this.configurationApiRequest = configurationApiRequest;
        }

        public async Task<ConfigGetByNameRequest.Response> Handle(ConfigGetByNameRequest request, CancellationToken cancellationToken)
        {
            return new ConfigGetByNameRequest.Response(
                await configurationApiRequest.GetConfigAsync(request.Name)
                .ConfigureAwait(false));
        }
    }
}
