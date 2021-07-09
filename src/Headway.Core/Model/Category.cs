using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Model
{
    [DynamicModelAttribute]
    public class Category : IPermissionable
    {
        public Category()
        {
            MenuItems = new List<MenuItem>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Permission { get; set; }
        public Module Module { get; set; }
        public List<MenuItem> MenuItems { get; set; }

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
