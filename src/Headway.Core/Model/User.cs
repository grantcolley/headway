using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class User
    {
        public User()
        {
            Permissions = new List<Permission>();
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public List<Permission> Permissions {get;set;}
    }
}
