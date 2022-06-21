using Headway.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class User
    {
        public User()
        {
            Roles = new List<Role>();
            Permissions = new List<Permission>();
        }

        public int UserId { get; set; }
        public List<Role> Roles { get; set; }
        public List<Permission> Permissions { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not a valid e-mail address.")]
        public string Email { get; set; }

        [NotMapped]
        public List<ChecklistItem> PermissionChecklist { get; set; }

        [NotMapped]
        public List<ChecklistItem> RoleChecklist { get; set; }

        [NotMapped]
        public List<string> RoleList
        {
            get
            {
                if (RoleChecklist == null)
                {
                    return new List<string>();
                }

                return RoleChecklist
                    .Where(r => r.IsChecked)
                    .Select(r => r.Name)
                    .ToList();
            }
        }

        [NotMapped]
        public List<string> PermissionList
        {
            get
            {
                if (PermissionChecklist == null
                    || RoleChecklist == null)
                {
                    return new List<string>();
                }

                var rolePermissions = RoleChecklist
                    .Where(r => r.IsChecked)
                    .SelectMany(r => r.SubItems)
                    .ToList();

                var assignedPermissions = PermissionChecklist
                    .Where(p => p.IsChecked)
                    .Select(r => r.Name)
                    .ToList();

                return assignedPermissions
                    .Union(rolePermissions)
                    .OrderBy(p => p)
                    .ToList();
            }
        }
    }
}
