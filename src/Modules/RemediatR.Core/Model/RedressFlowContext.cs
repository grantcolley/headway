using Headway.Core.Interface;
using Headway.Core.Model;
using System.Collections.Generic;
using System.Linq;

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
        public User CurrentUser { get; set; }
        public List<RedressFlowHistory> RedressFlowHistory { get; set; }

        public List<FlowHistory> GetFlowHistory()
        {
            return RedressFlowHistory.ToList<FlowHistory>();
        }
    }
}