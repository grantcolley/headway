using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public List<User> Users { get; set; }
        public List<Role> Roles { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Name between 1 and 20 characters")]
        public string Name { get; set; }
    }
}
