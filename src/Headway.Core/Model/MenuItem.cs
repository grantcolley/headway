using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class MenuItem
    {
        public MenuItem()
        {
            Permissions = new List<Permission>();
        }

        public int MenuItemId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string ImageClass { get; set; }
        public string Path { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}