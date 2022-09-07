using Headway.Core.Model;
using MediatR;

namespace Headway.RequestApi.Requests
{
    public record LogRequest(Log Log) : IRequest<Unit>
    {
    }
}
