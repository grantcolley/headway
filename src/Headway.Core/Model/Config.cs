using FluentValidation;
using Headway.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class Config : ModelBase
    {
        public Config()
        {
            ConfigItems = new List<ConfigItem>();
            ConfigContainers = new List<ConfigContainer>();
            ConfigSearchItems = new List<ConfigSearchItem>();
        }

        public int ConfigId { get; set; }
        public int? FlowId { get; set; }
        public bool NavigateResetBreadcrumb { get; set; }
        public bool CreateLocal { get; set; }
        public bool UseSearchComponent { get; set; }
        public List<ConfigItem> ConfigItems { get; set; }
        public List<ConfigContainer> ConfigContainers { get; set; }
        public List<ConfigSearchItem> ConfigSearchItems { get; set; }
        public Flow Flow { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(150)]
        public string Description { get; set; }

        [Required]
        [StringLength(150)]
        public string Model { get; set; }

        [Required]
        [StringLength(50)]
        public string ModelApi { get; set; }

        [StringLength(50)]
        public string OrderModelBy { get; set; }

        [StringLength(150)]
        public string Document { get; set; }

        [StringLength(350)]
        public string DocumentArgs { get; set; }

        [StringLength(50)]
        public string NavigatePage { get; set; }

        [StringLength(50)]
        public string NavigateProperty { get; set; }

        [StringLength(50)]
        public string NavigateConfig { get; set; }

        [StringLength(150)]
        public string SearchComponent { get; set; }
    }

    public class ConfigValidator : AbstractValidator<Config>
    {
        public ConfigValidator()
        {
            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Name cannot exceed 50 characters");

            RuleFor(v => v.Title)
                .NotNull().WithMessage("Title is required")
                .Length(1, 50).WithMessage("Title cannot exceed 50 characters");

            RuleFor(v => v.Description)
                .NotNull().WithMessage("Description is required")
                .Length(1, 150).WithMessage("Description cannot exceed 150 characters");

            RuleFor(v => v.Model)
                .NotNull().WithMessage("Model is required")
                .Length(1, 150).WithMessage("Model cannot exceed 150 characters");

            RuleFor(v => v.ModelApi)
                .NotNull().WithMessage("Model Api is required")
                .Length(1, 50).WithMessage("Model Api cannot exceed 50 characters");

            RuleFor(v => v.OrderModelBy)
                .Length(1, 50).WithMessage("Order Model By cannot exceed 50 characters");

            RuleFor(v => v.Document)
                .Length(1, 150).WithMessage("Document cannot exceed 150 characters");

            RuleFor(v => v.DocumentArgs)
                .Length(1, 350).WithMessage("Document Args cannot exceed 350 characters");

            RuleFor(v => v.NavigatePage)
                .Length(1, 150).WithMessage("Navigate Page cannot exceed 150 characters");

            RuleFor(v => v.NavigateProperty)
                .Length(1, 50).WithMessage("Navigate Property cannot exceed 50 characters");

            RuleFor(v => v.NavigateConfig)
                .Length(1, 50).WithMessage("Navigate Config cannot exceed 50 characters");

            RuleFor(v => v.SearchComponent)
                .Length(1, 150).WithMessage("Search Component cannot exceed 150 characters");
        }
    }
}
