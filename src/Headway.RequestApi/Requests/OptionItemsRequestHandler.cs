using Headway.Core.Interface;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Headway.RequestApi.Requests
{
    public class OptionItemsRequestHandler : IRequestHandler<OptionItemsRequest, OptionItemsRequest.Response>
    {
        private readonly IOptionsApiRequest optionsApiRequest;

        public OptionItemsRequestHandler(IOptionsApiRequest optionsApiRequest)
        {
            this.optionsApiRequest = optionsApiRequest;
        }

        public async Task<OptionItemsRequest.Response> Handle(OptionItemsRequest request, CancellationToken cancellationToken)
        {
            return new OptionItemsRequest.Response(await optionsApiRequest.GetOptionItemsAsync(request.DynamicArgs).ConfigureAwait(false));
        }
    }
}
