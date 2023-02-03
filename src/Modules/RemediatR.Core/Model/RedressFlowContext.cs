using Headway.Core.Interface;
using System.Collections.Generic;

namespace RemediatR.Core.Model
{
    public class RedressFlowContext : IFlowContext
    {
        public RedressFlowContext() 
        {
            RedressFlowHistory = new List<RedressFlowHistory>();
        }

        public int RedressFlowContextId { get; set; }
        public int FlowId { get; set; }
        public Headway.Core.Model.Flow Flow { get; set; }
        public int RedressId { get; set; }
        public Redress Redress { get; set; }
        public List<RedressFlowHistory> RedressFlowHistory { get; set; }
    }
}
