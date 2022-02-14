using Headway.Core.Interface;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Headway.Core.Mediators
{
    public class ModulesGetRequestHandler : IRequestHandler<ModulesGetRequest, ModulesGetRequest.Response>
    {
        private readonly IModuleService moduleService;

        public ModulesGetRequestHandler(IModuleService moduleService)
        {
            this.moduleService = moduleService;
        }

        public async Task<ModulesGetRequest.Response> Handle(ModulesGetRequest request, CancellationToken cancellationToken)
        {
            return new ModulesGetRequest.Response(await moduleService.GetModulesAsync().ConfigureAwait(false));
        }
    }
}