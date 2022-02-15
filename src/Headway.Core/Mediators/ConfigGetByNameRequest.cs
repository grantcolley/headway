using Headway.Core.Interface;
using Headway.Core.Model;
using MediatR;

namespace Headway.Core.Mediators
{
    public record ConfigGetByNameRequest(string Name) : IRequest<ConfigGetByNameRequest.Response>
    {
        public record Response(IResponse<Config> Config);
    }
}
