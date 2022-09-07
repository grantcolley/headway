using Headway.Core.Interface;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Headway.RequestApi.Requests
{
    public class LogRequestHandler : IRequestHandler<LogRequest, Unit>
    {
        private readonly ILogApiRequest logApiRequest;

        public LogRequestHandler(ILogApiRequest logApiRequest)
        {
            this.logApiRequest = logApiRequest;
        }

        public async Task<Unit> Handle(LogRequest request, CancellationToken cancellationToken)
        {
            await logApiRequest.LogAsync(request.Log).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
