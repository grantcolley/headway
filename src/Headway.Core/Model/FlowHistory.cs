using Headway.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    public class FlowHistory : ModelBase
    {
        [Required]
        public StateStatus StateStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string FlowCode { get; set; }

        [Required]
        [StringLength(50)]
        public string StateCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Event { get; set; }

        [Required]
        [StringLength(50)]
        public string Owner { get; set; }

        [StringLength(250)]
        public string Comment { get; set; }
    }
}
