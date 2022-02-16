using Headway.Core.Interface;
using Headway.Core.Model;
using MediatR;
using System.Collections.Generic;

namespace Headway.Core.Mediators
{
    public record OptionItemsRequest(List<DynamicArg> DynamicArgs) : IRequest<OptionItemsRequest.Response>
    {
        public record Response(IResponse<IEnumerable<OptionItem>> OptionItems);
    }
}
