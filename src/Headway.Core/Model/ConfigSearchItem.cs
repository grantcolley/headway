using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    public class ConfigSearchItem
    {
        public int ConfigSearchItemId { get; set; }
        public int Order { get; set; }

        [Required(ErrorMessage = "Label is required")]
        [StringLength(50, ErrorMessage = "Label must be between 1 and 50 characters")]
        public string Label { get; set; }

        [StringLength(50, ErrorMessage = "Tooltip cannot exceed 50 characters")]
        public string Tooltip { get; set; }

        [StringLength(150, ErrorMessage = "Component cannot exceed 150 characters")]
        public string Component { get; set; }

        [StringLength(350, ErrorMessage = "ComponentArgs must be between 1 and 350 characters")]
        public string ComponentArgs { get; set; }
    }
}