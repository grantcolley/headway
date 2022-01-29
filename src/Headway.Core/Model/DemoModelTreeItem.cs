using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class DemoModelTreeItem : IComponentTree
    {
        public DemoModelTreeItem()
        {
            DemoModelTreeItems = new List<DemoModelTreeItem>();
        }

        public int DemoModelTreeItemId { get; set; }
        public int Order { get; set; }
        public int DemoModelId { get; set; }
        public List<DemoModelTreeItem> DemoModelTreeItems { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [StringLength(50, ErrorMessage = "Code must be between 1 and 50 characters")]
        public string Code { get; set; }

        [StringLength(50, ErrorMessage = "ParentCode must be between 1 and 50 characters")]
        public string ParentCode { get; set; }
    }
}
