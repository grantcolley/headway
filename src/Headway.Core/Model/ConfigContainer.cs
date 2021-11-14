using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    public class ConfigContainer
    {
        public ConfigContainer()
        {
            ConfigContainers = new List<ConfigContainer>();
        }

        public int ConfigContainerId { get; set; }
        public bool IsRootContainer { get; set; }
        public int? Row { get; set; }
        public int? Column { get; set; }
        public int Order { get; set; }

        public List<ConfigContainer> ConfigContainers { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Container is required")]
        [StringLength(150, ErrorMessage = "Container must be between 1 and 150 characters")]
        public string Container { get; set; }

        [StringLength(50, ErrorMessage = "Test must be between 1 and 50 characters")]
        public string Text { get; set; }
    }
}