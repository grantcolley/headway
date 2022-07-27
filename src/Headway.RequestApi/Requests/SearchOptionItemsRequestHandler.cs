using Headway.Core.Interface;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Headway.RequestApi.Requests
{
    public class SearchOptionItemsRequestHandler : IRequestHandler<SearchOptionItemsRequest, SearchOptionItemsRequest.Response>
    {
        private readonly IOptionsApiRequest optionsApiRequest;

        public SearchOptionItemsRequestHandler(IOptionsApiRequest optionsApiRequest)
        {
            this.optionsApiRequest = optionsApiRequest;
        }

        public async Task<SearchOptionItemsRequest.Response> Handle(SearchOptionItemsRequest request, CancellationToken cancellationToken)
        {
            return new SearchOptionItemsRequest.Response(await optionsApiRequest.GetOptionItemsAsync(request.Args).ConfigureAwait(false));
        }
    }
}
