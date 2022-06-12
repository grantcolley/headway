using Headway.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Headway.Core.Model
{
    [DynamicModel]
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

        [NotMapped]
        public string UserList
        {
            get
            {
                if (Users == null)
                {
                    return string.Empty;
                }

                return string.Join(", ",
                    Users.Select(u => u.Email)
                    .OrderBy(e => e)
                    .Distinct());
            }
        }

        [NotMapped]
        public string RoleList
        {
            get
            {
                if (Roles == null)
                {
                    return string.Empty;
                }

                return string.Join(", ",
                    Roles.Select(r => r.Name)
                    .OrderBy(n => n)
                    .Distinct());
            }
        }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(150, ErrorMessage = "Description must be between 1 and 150 characters")]
        public string Description { get; set; }
    }
}
