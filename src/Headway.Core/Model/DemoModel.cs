using FluentValidation;
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
        public DemoModel()
        {
            DemoModelItems = new List<DemoModelItem>();
            DemoModelTreeItems = new List<DemoModelTreeItem>();
        }

        public int DemoModelId { get; set; }
        public bool Checkbox { get; set; }
        public int Integer { get; set; }
        public string OptionVertical { get; set; }
        public string OptionHorizontal { get; set; }
        public string Dropdown { get; set; }
        public DateTime Date { get; set; }
        public DemoModelComplexProperty DropdownComplex { get; set; }
        public List<DemoModelItem> DemoModelItems { get; set; }
        public List<DemoModelTreeItem> DemoModelTreeItems { get; set; }

        [Required]
        [StringLength(20)]
        public string Text { get; set; }

        [StringLength(50)]
        public string TextMultiline { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Decimal { get; set; }
    }

    public class DemoModelValidator : AbstractValidator<DemoModel>
    {
        public DemoModelValidator()
        {
            RuleFor(v => v.Text)
                .NotNull().WithMessage("Text is required")
                .Length(1, 20).WithMessage("Text cannot exceed 20 characters");

            RuleFor(v => v.TextMultiline)
                .Length(1, 50).WithMessage("TextMultiline cannot exceed 50 characters");
        }
    }
}
