using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface IFlowContext
    {
        int FlowId { get; set; }
        Flow Flow { get; set; }
        User CurrentUser { get; set; }
        List<FlowHistory> GetFlowHistory();
    }
}
