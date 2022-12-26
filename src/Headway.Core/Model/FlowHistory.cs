using Headway.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    public class FlowHistory : ModelBase
    {
        public int FlowHistoryId { get; set; }
        public StateStatus StateStatus { get; set; }
        public Flow Flow { get; set; }

        [StringLength(50)]
        public string StateCode { get; set; }

        [StringLength(50)]
        public string Event { get; set; }

        [StringLength(50)]
        public string Owner { get; set; }

        [StringLength(50)]
        public string Ascendant { get; set; }

        [StringLength(50)]
        public string Descendant { get; set; }

        [StringLength(250)]
        public string Comment { get; set; }
    }
}
