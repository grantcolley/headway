using Headway.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Headway.RequestApi.Requests
{
    public record ModulesGetRequest : IRequest<ModulesGetRequest.Response>
    {
        public record Response(IEnumerable<Module> Modules);
    }
}
