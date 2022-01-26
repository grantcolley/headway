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
        public int ConfigId { get; set; }
        public bool IsRootContainer { get; set; }
        public int Order { get; set; }

        public List<ConfigContainer> ConfigContainers { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Container is required")]
        [StringLength(150, ErrorMessage = "Container must be between 1 and 150 characters")]
        public string Container { get; set; }

        [Required(ErrorMessage = "ContainerCode is required")]
        [StringLength(20, ErrorMessage = "ContainerCode must be between 1 and 20 characters")]
        public string ContainerCode { get; set; }

        [StringLength(20, ErrorMessage = "ParentContainerCode must be between 1 and 20 characters")]
        public string ParentContainerCode { get; set; }

        [StringLength(50, ErrorMessage = "Label must be between 1 and 50 characters")]
        public string Label { get; set; }
    }
}