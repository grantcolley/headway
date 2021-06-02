using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorAdmin.Model;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class UserDetailsBase : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public int UserId { get; set; }

        protected User user { get; set; }

        protected List<HeadwayRole> headwayRoles { get; set; }

        protected List<HeadwayPermission> headwayPermissions { get; set; }

        protected Alert alert { get; set; }

        protected bool IsSaveInProgress = false;
        protected bool IsDeleteInProgress = false;

        protected override async Task OnInitializedAsync()
        {
            if (UserId.Equals(0))
            {
                user = new User();
            }
            else
            {
                var userResponse = await AuthorisationService.GetUserAsync(UserId).ConfigureAwait(false);
                user = GetResponse(userResponse);
                if (user == null)
                {
                    return;
                }
            }
            
            var permissionsResponse = await AuthorisationService.GetPermissionsAsync().ConfigureAwait(false);
            var permissions = GetResponse(permissionsResponse);
            if (permissions == null)
            {
                return;
            }

            headwayPermissions = (from p in permissions
                                  join up in user.Permissions on p.PermissionId equals up.PermissionId into j
                                  from res in j.DefaultIfEmpty()
                                  select new HeadwayPermission(p) { IsSelected = res == null ? false : true }).ToList();

            var rolesResponse = await AuthorisationService.GetRolesAsync().ConfigureAwait(false);
            var roles = GetResponse(rolesResponse);
            if(roles == null)
            {
                return;
            }

            headwayRoles = (from r in roles
                            join ur in user.Roles on r.RoleId equals ur.RoleId into j
                            from res in j.DefaultIfEmpty()
                            select new HeadwayRole(r) { IsSelected = res == null ? false : true }).ToList();

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task SubmitUser()
        {
            IsSaveInProgress = true;

            if (user.UserId.Equals(0))
            {
                var userResponse = await AuthorisationService.AddUserAsync(user).ConfigureAwait(false);
                user = GetResponse(userResponse);
                if(user == null)
                {
                    return;
                }

                alert = new Alert
                {
                    AlertType = "primary",
                    Title = $"{user.UserName}",
                    Message = $"has been added.",
                    RedirectText = "Return to users.",
                    RedirectPage = "/Users"
                };
            }
            else
            {
                var userResponse = await AuthorisationService.UpdateUserAsync(user).ConfigureAwait(false);
                user = GetResponse(userResponse);
                if (user == null)
                {
                    return;
                }

                alert = new Alert
                {
                    AlertType = "primary",
                    Title = $"{user.UserName}",
                    Message = $"has been updated.",
                    RedirectText = "Return to users.",
                    RedirectPage = "/users"
                };
            }

            IsSaveInProgress = false;
        }

        protected async Task DeleteUser(User user)
        {
            IsDeleteInProgress = true;

            var deleteResponse = await AuthorisationService.DeleteUserAsync(user.UserId).ConfigureAwait(false);
            var deleteResult = GetResponse(deleteResponse);
            if (deleteResult.Equals(0))
            {
                return;
            }

            alert = new Alert
            {
                AlertType = "danger",
                Title = $"{user.UserName}",
                Message = $"has been deleted.",
                RedirectText = "Return to users.",
                RedirectPage = "/users"
            };

            IsDeleteInProgress = false;
        }

        protected void RolesCheckboxClicked(int roleId, object checkedValue)
        {           
            bool isChecked = (bool)checkedValue;

            if (isChecked)
            {
                var role = headwayRoles.Single(r => r.Role.RoleId.Equals(roleId));
                user.Roles.Add(new Role
                {
                    RoleId = role.Role.RoleId,
                    Name = role.Name,
                    Description = role.Description
                });
            }
            else if (!isChecked)
            {
                var userRole = user.Roles.SingleOrDefault(r => r.RoleId.Equals(roleId));
                user.Roles.Remove(userRole);
            }
        }

        protected void PermissionCheckboxClicked(int permissionId, object checkedValue)
        {  
            bool isChecked = (bool)checkedValue;

            if(isChecked)
            {
                var permission = headwayPermissions.Single(p => p.Permission.PermissionId.Equals(permissionId));
                user.Permissions.Add(new Permission
                {
                    PermissionId = permission.Permission.PermissionId,
                    Name = permission.Name,
                    Description = permission.Description
                });
            }
            else if(!isChecked)
            {
                var userPermission = user.Permissions.SingleOrDefault(p => p.PermissionId.Equals(permissionId));
                user.Permissions.Remove(userPermission);
            }
        }
    }
}
