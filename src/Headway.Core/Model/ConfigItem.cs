using Headway.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class ConfigItem
    {
        public int ConfigItemId { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsTitle { get; set; }
        public int Order { get; set; }
        public string ComponentArgs { get; set; }
        public string ConfigName { get; set; }
        public ConfigContainer ConfigContainer { get; set; }

        [NotMapped]
        public Config Config {  get; set; }

        [Required(ErrorMessage = "Property Name is required")]
        [StringLength(50, ErrorMessage = "Property Name must be between 1 and 50 characters")]
        public string PropertyName { get; set; }

        [Required(ErrorMessage = "Label is required")]
        [StringLength(50, ErrorMessage = "Label must be between 1 and 50 characters")]
        public string Label { get; set; }

        [StringLength(50, ErrorMessage = "Tooltip cannot exceed 50 characters")]
        public string Tooltip { get; set; }

        [StringLength(150, ErrorMessage = "Component cannot exceed 150 characters")]
        public string Component { get; set; }
    }
}