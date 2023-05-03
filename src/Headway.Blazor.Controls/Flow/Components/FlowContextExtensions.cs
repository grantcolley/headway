using Headway.Core.Interface;

namespace Headway.Blazor.Controls.Flow.Components
{
    public static class FlowContextExtensions
    {
        public static FlowComponentContext GetFlowComponentContext(this IFlowContext flowContext)
        {
            return new FlowComponentContext(flowContext);
        }
    }
}
