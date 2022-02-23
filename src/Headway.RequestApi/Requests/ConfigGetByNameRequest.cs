using Headway.Core.Interface;
using Headway.Core.Model;
using MediatR;

namespace Headway.RequestApi.Requests
{
    public record ConfigGetByNameRequest(string Name) : IRequest<ConfigGetByNameRequest.Response>
    {
        public record Response(IResponse<Config> Config);
    }
}
