using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface IFlowContext
    {
        int FlowId { get; set; }
        Flow Flow { get; set; }
        List<FlowHistory> GetFlowHistory();
        Authorisation Authorisation { get; set; }
    }
}
