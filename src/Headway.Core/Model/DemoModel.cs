using Headway.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class DemoModel
    {
        public int DemoModelId { get; set; }
        public bool Enabled { get; set; }
        public int Order { get; set; }

        public List<DemoModel> DemoModels { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Label must be between 1 and 50 characters")]
        public string Description { get; set; }

        [StringLength(50, ErrorMessage = "Label must be between 1 and 50 characters")]
        public string Style { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
