using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    public class ConfigItem
    {
        public int ConfigItemId { get; set; }
        public LayoutConfigItem LayoutConfigItems { get; set; }
        public bool? IsIdentity { get; set; }
        public bool? IsTitle { get; set; }
        public int Order { get; set; }

        [Required(ErrorMessage = "Property Name is required.")]
        [StringLength(50, ErrorMessage = "Property Name must be between 1 and 50 characters")]
        public string PropertyName { get; set; }

        [Required(ErrorMessage = "Label is required.")]
        [StringLength(50, ErrorMessage = "Label must be between 1 and 50 characters")]
        public string Label { get; set; }

        [StringLength(100, ErrorMessage = "Component cannot exceed 100 characters")]
        public string Component { get; set; }
    }
}
