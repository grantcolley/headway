using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.Core.Model
{
    public class FlowInstance 
    {
        public int FlowInstanceId { get; set; }

        public int RelatedInstanceId { get; set; }

        public int FlowId { get; set; }

        public string FlowHistory { get; set; }

        [NotMapped]
        public Flow Flow { get; set; }
    }
}
