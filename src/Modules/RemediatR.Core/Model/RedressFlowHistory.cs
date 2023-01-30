using Headway.Core.Model;

namespace RemediatR.Core.Model
{
    public class RedressFlowHistory : FlowHistory
    {
        public int RedressFlowHistoryId { get; set; }
        public int RedressId { get; set; }
        public Redress Redress { get; set; }
    }
}
