using Headway.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    public abstract class FlowHistory : ModelBase
    {
        public StateStatus StateStatus { get; set; }

        [StringLength(50)]
        public string StateCode { get; set; }

        [StringLength(50)]
        public string Owner { get; set; }

        [StringLength(50)]
        public string Ascendant { get; set; }

        [StringLength(50)]
        public string Descendant { get; set; }
    }
}
