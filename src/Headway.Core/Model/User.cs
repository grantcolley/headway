using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class User
    {
        public User()
        {
            Roles = new List<Role>();
            Permissions = new List<Permission>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<Role> Roles { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
