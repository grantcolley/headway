using Headway.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Headway.Core.Mediators
{
    public record ModulesGetRequest : IRequest<ModulesGetRequest.Response>
    {
        public record Response(IEnumerable<Module> Modules);
    }
}
