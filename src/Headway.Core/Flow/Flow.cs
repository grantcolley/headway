using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Flow
{
    public class Flow
    {
        public Flow()
        {
            States = new List<State>();
        }

        public int FlowId { get; set; }
        public List<State> States { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string Model { get; set; }

        [Required]
        [StringLength(50)]
        public string Permission { get; set; }

        [StringLength(50)]
        public string ActiveStateCode { get; set; }
    }
}