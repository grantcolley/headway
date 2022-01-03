using Headway.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class DemoModelItem
    {
        public int DemoModelItemId { get; set; }
        public int Order { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }
    }
}
