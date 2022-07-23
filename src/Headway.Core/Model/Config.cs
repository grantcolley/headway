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
            ConfigContainers = new List<ConfigContainer>();
            ConfigSearchItems = new List<ConfigSearchItem>();
        }

        public int ConfigId { get; set; }
        public bool NavigateResetBreadcrumb { get; set; }
        public bool CreateLocal { get; set; }
        public List<ConfigItem> ConfigItems { get; set; }
        public List<ConfigContainer> ConfigContainers { get; set; }
        public List<ConfigSearchItem> ConfigSearchItems { get; set; }

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

        [StringLength(150, ErrorMessage = "DocumentArgs must be between 1 and 350 characters")]
        public string DocumentArgs { get; set; }

        [StringLength(50, ErrorMessage = "Navigate Page cannot exceed 50 characters")]
        public string NavigatePage { get; set; }

        [StringLength(50, ErrorMessage = "Navigate Property cannot exceed 50 characters")]
        public string NavigateProperty { get; set; }

        [StringLength(50, ErrorMessage = "Navigate Config cannot exceed 50 characters")]
        public string NavigateConfig { get; set; }
    }
}
