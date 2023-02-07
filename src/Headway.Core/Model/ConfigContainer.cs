using FluentValidation;
using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class ConfigContainer : ModelBase, IComponentTree, IComponent
    {
        public ConfigContainer()
        {
            ConfigContainers = new List<ConfigContainer>();
        }

        public int ConfigContainerId { get; set; }
        public int ConfigId { get; set; }
        public int Order { get; set; }

        public List<ConfigContainer> ConfigContainers { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string Container { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        public string ParentCode { get; set; }

        [StringLength(50)]
        public string Label { get; set; }

        [StringLength(350)]
        public string ComponentArgs { get; set; }

        [StringLength(350)]
        public string FlowArgs { get; set; }
    }

    public class ConfigContainerValidator : AbstractValidator<ConfigContainer>
    {
        public ConfigContainerValidator()
        {
            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Name cannot exceed 50 characters");

            RuleFor(v => v.Container)
                .NotNull().WithMessage("Container is required")
                .Length(1, 150).WithMessage("Container cannot exceed 150 characters");

            RuleFor(v => v.Code)
                .NotNull().WithMessage("Code is required")
                .Length(1, 50).WithMessage("Code cannot exceed 50 characters");

            RuleFor(v => v.ParentCode)
                .Length(1, 50).WithMessage("ParentCode cannot exceed 50 characters");

            RuleFor(v => v.Label)
                .Length(1, 50).WithMessage("Label cannot exceed 50 characters");

            RuleFor(v => v.ComponentArgs)
                .Length(1, 350).WithMessage("Component Args cannot exceed 350 characters");

            RuleFor(v => v.FlowArgs)
                .Length(1, 350).WithMessage("Flow Args cannot exceed 350 characters");
        }
    }
}