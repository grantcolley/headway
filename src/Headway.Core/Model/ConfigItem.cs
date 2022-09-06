using FluentValidation;
using Headway.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class ConfigItem : ModelBase
    {
        public int ConfigItemId { get; set; }
        public int ConfigId { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsTitle { get; set; }
        public int Order { get; set; }
        
        public ConfigContainer ConfigContainer { get; set; }

        [NotMapped]
        public Config Config {  get; set; }

        [Required]
        [StringLength(50)]
        public string PropertyName { get; set; }

        [Required]
        [StringLength(50)]
        public string Label { get; set; }

        [StringLength(50)]
        public string Tooltip { get; set; }

        [StringLength(150)]
        public string Component { get; set; }

        [StringLength(350)]
        public string ComponentArgs { get; set; }

        [StringLength(50)]
        public string ConfigName { get; set; }
    }

    public class ConfigItemValidator : AbstractValidator<ConfigItem>
    {
        public ConfigItemValidator()
        {
            RuleFor(v => v.PropertyName)
                .NotNull().WithMessage("Property Name is required")
                .Length(1, 50).WithMessage("Property Name cannot exceed 50 characters");

            RuleFor(v => v.Label)
                .NotNull().WithMessage("Label is required")
                .Length(1, 50).WithMessage("Label cannot exceed 50 characters");

            RuleFor(v => v.Tooltip)
                .Length(1, 50).WithMessage("Tooltip cannot exceed 50 characters");

            RuleFor(v => v.Component)
                .Length(1, 150).WithMessage("Component cannot exceed 150 characters");

            RuleFor(v => v.ComponentArgs)
                .Length(1, 350).WithMessage("Component Args cannot exceed 350 characters");

            RuleFor(v => v.ConfigName)
                .Length(1, 50).WithMessage("Config Name cannot exceed 50 characters");
        }
    }
}