using Headway.Core.Interface;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Headway.Core.Mediators
{
    public class ModulesRequestHandler : IRequestHandler<ModulesRequest, ModulesRequest.Response>
    {
        private readonly IModuleService moduleService;

        public ModulesRequestHandler(IModuleService moduleService)
        {
            this.moduleService = moduleService;
        }

        public async Task<ModulesRequest.Response> Handle(ModulesRequest request, CancellationToken cancellationToken)
        {
            return new ModulesRequest.Response(await moduleService.GetModulesAsync().ConfigureAwait(false));
        }
    }
}