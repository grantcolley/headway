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
        public string Description { get; set; }
        public bool Checkbox { get; set; }
        public int Integer { get; set; }
        public string OptionVertical { get; set; }
        public string OptionHorizontal { get; set; }
        public DateTime Date { get; set; }

        public List<DemoModel> DemoModels { get; set; }

        [Required(ErrorMessage = "Text is required")]
        [StringLength(50, ErrorMessage = "Text must be between 1 and 50 characters")]
        public string Text { get; set; }

        [StringLength(50, ErrorMessage = "TextMultiline must be between 1 and 50 characters")]
        public string TextMultiline { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Decimal { get; set; }
    }
}
