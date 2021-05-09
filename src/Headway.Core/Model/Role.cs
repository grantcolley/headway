using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class Role
    {
        public Role()
        {
            Permissions = new List<Permission>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
