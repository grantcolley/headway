using Headway.Core.Model;

namespace Headway.Core.Interface
{
    public interface IFlowContext
    {
        int FlowId { get; set; }
        Flow Flow { get; set; }
    }
}
