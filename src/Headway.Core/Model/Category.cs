using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.ComponentModel.DataAnnotations;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class Category : IPermissionable
    {
        public Category()
        {
            MenuItems = new List<MenuItem>();
        }

        public int CategoryId { get; set; }
        public int Order { get; set; }
        public Module Module { get; set; }
        public List<MenuItem> MenuItems { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, ErrorMessage = "Name must be between 1 and 20 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Permission is required")]
        [StringLength(20, ErrorMessage = "Permission must be between 1 and 20 characters")]
        public string Permission { get; set; }

        public bool IsPermitted(IEnumerable<string> permissions)
        {
            if(permissions.Contains(Permission))
            {
                foreach (var menuItem in MenuItems)
                {
                    menuItem.IsPermitted(permissions);
                }

                return true;
            }
            else
            {
                MenuItems.Clear();
                Module.Categories.Remove(this);
                return false;
            }
        }
    }
}
