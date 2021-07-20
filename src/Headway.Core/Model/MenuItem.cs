using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class MenuItem : IPermissionable
    {
        public int MenuItemId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string ImageClass { get; set; }
        public string Path { get; set; }
        public string Config { get; set; }
        public string Permission { get; set; }
        public Category Category { get; set; }

        public string NavigateTo()
        {
            return $@"{Path}\{Config}";
        }

        public bool IsPermitted(IEnumerable<string> permissions)
        {
            if (permissions.Contains(Permission))
            {
                return true;
            }
            else
            {
                Category.MenuItems.Remove(this);
                return false;
            }
        }
    }
}