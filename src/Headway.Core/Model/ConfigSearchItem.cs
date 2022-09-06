using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    public class ConfigSearchItem : ModelBase
    {
        public int ConfigSearchItemId { get; set; }
        public int Order { get; set; }

        [Required]
        [StringLength(50)]
        public string Label { get; set; }

        [Required]
        [StringLength(50)]
        public string ParameterName { get; set; }

        [StringLength(50)]
        public string Tooltip { get; set; }

        [StringLength(150)]
        public string Component { get; set; }

        [StringLength(350)]
        public string ComponentArgs { get; set; }
    }

    public class ConfigSearchItemValidator : AbstractValidator<ConfigSearchItem>
    {
        public ConfigSearchItemValidator()
        {
            RuleFor(v => v.ParameterName)
                .NotNull().WithMessage("Parameter Name is required")
                .Length(1, 50).WithMessage("Parameter Name cannot exceed 50 characters");

            RuleFor(v => v.Label)
                .NotNull().WithMessage("Label is required")
                .Length(1, 50).WithMessage("Label cannot exceed 50 characters");

            RuleFor(v => v.Tooltip)
                .Length(1, 50).WithMessage("Tooltip cannot exceed 50 characters");

            RuleFor(v => v.Component)
                .Length(1, 150).WithMessage("Component cannot exceed 150 characters");

            RuleFor(v => v.ComponentArgs)
                .Length(1, 350).WithMessage("Component Args cannot exceed 350 characters");
        }
    }
}