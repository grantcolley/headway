﻿using FluentValidation;
using Headway.Core.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class User : ModelBase
    {
        private List<string> userPermissions;

        public User()
        {
            Roles = new List<Role>();
            Permissions = new List<Permission>();
            PermissionChecklist = new List<ChecklistItem>();
            RoleChecklist = new List<ChecklistItem>();
        }

        public int UserId { get; set; }
        public List<Role> Roles { get; set; }
        public List<Permission> Permissions { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [NotMapped]
        public List<ChecklistItem> PermissionChecklist { get; set; }

        [NotMapped]
        public List<ChecklistItem> RoleChecklist { get; set; }

        [NotMapped]
        [JsonIgnore]
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
        [JsonIgnore]
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

        public List<string> UserPermissions
        {
            get
            {
                if(userPermissions != null)
                {
                    return userPermissions;
                }

                userPermissions = Permissions
                    .Select(p => p.Name)
                    .Union(Roles.SelectMany(r => r.Permissions.Select(rp => rp.Name)))
                    .ToList();

                return userPermissions;
            }
        }
    }

    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(v => v.UserName)
                .NotNull().WithMessage("User Name is required")
                .Length(1, 50).WithMessage("User Name cannot exceed 50 characters");

            RuleFor(v => v.Email)
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required");
        }
    }
}
