using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class Permission
    {
        public Permission()
        {
            Users = new List<User>();
            Roles = new List<Role>();
        }

        public int PermissionId { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public List<Role> Roles { get; set; }
    }
}
