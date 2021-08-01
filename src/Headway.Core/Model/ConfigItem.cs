using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    public class ConfigItem
    {
        public int ConfigItemId { get; set; }
        public bool? IsIdentity { get; set; }
        public bool? IsTitle { get; set; }
        public int Order { get; set; }
        public string Component { get; set; }

        [Required(ErrorMessage = "Property Name is required.")]
        [StringLength(20, ErrorMessage = "Property Name must be between 1 and 20 characters")]
        public string PropertyName { get; set; }

        [Required(ErrorMessage = "Label is required.")]
        [StringLength(20, ErrorMessage = "Label must be between 1 and 20 characters")]
        public string Label { get; set; }

    }
}
