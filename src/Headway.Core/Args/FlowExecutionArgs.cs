using Headway.Core.Enums;

namespace Headway.Core.Args
{
    public class FlowExecutionArgs
    {
        public FlowActionEnum FlowAction { get; set; }
        public StateStatus StateStatus { get; set; }
        public string StateCode { get; set; }
        public string TargetStateCode { get; set; }
        public string Comment { get; set; }
    }
}
