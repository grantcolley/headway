using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    public class ConfigContainer
    {
        public int ConfigContainerId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public List<ConfigContainer> ConfigContainers { get; set; }

        [Required(ErrorMessage = "Container is required.")]
        [StringLength(150, ErrorMessage = "Container must be between 1 and 150 characters")]
        public string Container { get; set; }
    }
}