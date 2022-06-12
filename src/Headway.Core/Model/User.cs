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

        [NotMapped]
        public string PermissionList
        {
            get
            {
                if (Permissions == null)
                {
                    return string.Empty;
                }

                return string.Join(", ",
                    Permissions.Select(p => p.Name)
                    .OrderBy(n => n)
                    .Distinct());
            }
        }

        [NotMapped]
        public List<ChecklistItem> PermissionChecklist { get; set; }

        [NotMapped]
        public List<ChecklistItem> RoleChecklist { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not a valid e-mail address.")]
        public string Email { get; set; }
    }
}
