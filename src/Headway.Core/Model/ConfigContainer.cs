using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class ConfigContainer : IComponentTree, IComponent
    {
        public ConfigContainer()
        {
            ConfigContainers = new List<ConfigContainer>();
        }

        public int ConfigContainerId { get; set; }
        public int ConfigId { get; set; }
        public int Order { get; set; }

        public List<ConfigContainer> ConfigContainers { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Container is required")]
        [StringLength(150, ErrorMessage = "Container must be between 1 and 150 characters")]
        public string Container { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [StringLength(50, ErrorMessage = "Code must be between 1 and 50 characters")]
        public string Code { get; set; }

        [StringLength(50, ErrorMessage = "ParentCode must be between 1 and 50 characters")]
        public string ParentCode { get; set; }

        [StringLength(50, ErrorMessage = "Label must be between 1 and 50 characters")]
        public string Label { get; set; }

        [StringLength(350, ErrorMessage = "ComponentArgs must be between 1 and 350 characters")]
        public string ComponentArgs { get; set; }
    }
}