using Headway.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class DemoModelItem
    {
        public int DemoModelItemId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(50, ErrorMessage = "Description must be between 1 and 50 characters")]
        public string Description { get; set; }
    }
}
