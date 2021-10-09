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
        [StringLength(20, ErrorMessage = "Name must be between 1 and 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(20, ErrorMessage = "Title must be between 1 and 20 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [StringLength(150, ErrorMessage = "Model must be between 1 and 150 characters")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Model Api is required")]
        [StringLength(20, ErrorMessage = "Model Api must be between 1 and 20 characters")]
        public string ModelApi { get; set; }

        [StringLength(50, ErrorMessage = "Order Model By must be between 1 and 50 characters")]
        public string OrderModelBy { get; set; }

        [Required(ErrorMessage = "Container is required")]
        [StringLength(150, ErrorMessage = "Container must be between 1 and 150 characters")]
        public string Container { get; set; }

        [StringLength(20, ErrorMessage = "Navigate To cannot exceed 20 characters")]
        public string NavigateTo { get; set; }

        [StringLength(50, ErrorMessage = "Navigate To Property cannot exceed 50 characters")]
        public string NavigateToProperty { get; set; }

        [StringLength(20, ErrorMessage = "Navigate To Config cannot exceed 20 characters")]
        public string NavigateToConfig { get; set; }

        [StringLength(20, ErrorMessage = "Navigate Back cannot exceed 20 characters")]
        public string NavigateBack { get; set; }

        [StringLength(50, ErrorMessage = "Navigate Back Property cannot exceed 50 characters")]
        public string NavigateBackProperty { get; set; }

        [StringLength(20, ErrorMessage = "Navigate Back Config cannot exceed 20 characters")]
        public string NavigateBackConfig { get; set; }
    }
}
