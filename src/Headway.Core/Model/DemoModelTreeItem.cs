using FluentValidation;
using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class DemoModelTreeItem : ModelBase, IComponentTree
    {
        public DemoModelTreeItem()
        {
            DemoModelTreeItems = new List<DemoModelTreeItem>();
        }

        public int DemoModelTreeItemId { get; set; }
        public int Order { get; set; }
        public int DemoModelId { get; set; }
        public List<DemoModelTreeItem> DemoModelTreeItems { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string ParentCode { get; set; }
    }

    public class DemoModelTreeItemValidator : AbstractValidator<DemoModelTreeItem>
    {
        public DemoModelTreeItemValidator()
        {
            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Name cannot exceed 50 characters");

            RuleFor(v => v.Code)
                .NotNull().WithMessage("Code is required")
                .Length(1, 50).WithMessage("Code cannot exceed 50 characters");

            RuleFor(v => v.ParentCode)
                .Length(1, 50).WithMessage("Parent Code cannot exceed 50 characters");
        }
    }
}
