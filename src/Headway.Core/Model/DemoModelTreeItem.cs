using Headway.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class DemoModelTreeItem
    {
        public DemoModelTreeItem()
        {
            DemoModelTreeItems = new List<DemoModelTreeItem>();
        }

        public int DemoModelTreeItemId { get; set; }
        public int Order { get; set; }
        public List<DemoModelTreeItem> DemoModelTreeItems { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }
    }
}
