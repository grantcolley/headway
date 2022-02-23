using Headway.Core.Interface;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Headway.RequestApi.Requests
{
    public class ModulesGetRequestHandler : IRequestHandler<ModulesGetRequest, ModulesGetRequest.Response>
    {
        private readonly IModuleApiRequest moduleApiRequest;

        public ModulesGetRequestHandler(IModuleApiRequest moduleApiRequest)
        {
            this.moduleApiRequest = moduleApiRequest;
        }

        public async Task<ModulesGetRequest.Response> Handle(ModulesGetRequest request, CancellationToken cancellationToken)
        {
            return new ModulesGetRequest.Response(await moduleApiRequest.GetModulesAsync().ConfigureAwait(false));
        }
    }
}