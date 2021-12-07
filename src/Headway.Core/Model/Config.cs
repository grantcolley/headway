using Headway.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class Config
    {
        public Config()
        {
            ConfigItems = new List<ConfigItem>();
            Containers = new List<ConfigContainer>();
        }

        public int ConfigId { get; set; }
        public List<ConfigItem> ConfigItems { get; set; }
        public List<ConfigContainer> Containers { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(50, ErrorMessage = "Title must be between 1 and 50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(150, ErrorMessage = "Description must be between 1 and 150 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [StringLength(150, ErrorMessage = "Model must be between 1 and 150 characters")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Model Api is required")]
        [StringLength(50, ErrorMessage = "Model Api must be between 1 and 50 characters")]
        public string ModelApi { get; set; }

        [StringLength(50, ErrorMessage = "Order Model By must be between 1 and 50 characters")]
        public string OrderModelBy { get; set; }

        [StringLength(150, ErrorMessage = "Document must be between 1 and 150 characters")]
        public string Document { get; set; }

        [StringLength(50, ErrorMessage = "Navigate To cannot exceed 50 characters")]
        public string NavigateTo { get; set; }

        [StringLength(50, ErrorMessage = "Navigate To Property cannot exceed 50 characters")]
        public string NavigateToProperty { get; set; }

        [StringLength(50, ErrorMessage = "Navigate To Config cannot exceed 50 characters")]
        public string NavigateToConfig { get; set; }
    }
}
