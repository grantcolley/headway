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
        }

        public int ConfigId { get; set; }
        public ConfigType ConfigType { get; set; }
        public string NavigateTo { get; set; }
        public string NavigateToProperty { get; set; }
        public string NavigateToConfig { get; set; }
        public string NavigateBack { get; set; }
        public string NavigateBackProperty { get; set; }
        public string NavigateBackConfig { get; set; }
        public List<ConfigItem> ConfigItems { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, ErrorMessage = "Name must be between 1 and 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(20, ErrorMessage = "Title must be between 1 and 20 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        [StringLength(20, ErrorMessage = "Model must be between 1 and 20 characters")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Model Api is required.")]
        [StringLength(20, ErrorMessage = "Model Api must be between 1 and 20 characters")]
        public string ModelApi { get; set; }

        [Required(ErrorMessage = "Component is required.")]
        [StringLength(100, ErrorMessage = "Component must be between 1 and 20 characters")]
        public string Component { get; set; }
    }
}
