using Headway.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Headway.Core.Mediators
{
    public record ModulesRequest : IRequest<ModulesRequest.Response>
    {
        public record Response(IEnumerable<Module> Modules);
    }
}
