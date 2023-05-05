using Headway.Core.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.Core.Interface
{
    public interface IFlowContext
    {
        int FlowId { get; set; }
        Flow Flow { get; set; }
        List<FlowHistory> GetFlowHistory();

        [NotMapped]
        User CurrentUser { get; set; }
    }
}
