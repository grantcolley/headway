using Headway.Core.Model;
using System.Collections.Generic;

namespace Headway.Core.Interface
{
    public interface IFlowContext
    {
        Flow Flow { get; set; }
        List<FlowHistory> FlowHistory { get; set; }
    }
}
