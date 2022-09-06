using FluentValidation;
using Headway.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class DemoModelItem : ModelBase
    {
        public int DemoModelItemId { get; set; }
        public int DemoModelId { get; set; }
        public int Order { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }

    public class DemoModelItemValidator : AbstractValidator<DemoModelItem>
    {
        public DemoModelItemValidator()
        {
            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Name cannot exceed 50 characters");
        }
    }
}
