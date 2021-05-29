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
    public class UserDetailsBase : ComponentBase
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
                user = await AuthorisationService.GetUserAsync(UserId).ConfigureAwait(false);
            }

            var permissions = await AuthorisationService.GetPermissionsAsync().ConfigureAwait(false);
            headwayPermissions = (from p in permissions
                                 join up in user.Permissions on p.PermissionId equals up.PermissionId into j
                                 from res in j.DefaultIfEmpty()
                                 select new HeadwayPermission(p) { IsSelected = res == null ? false : true }).ToList();

            var roles = await AuthorisationService.GetRolesAsync().ConfigureAwait(false);
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
                user = await AuthorisationService.AddUserAsync(user).ConfigureAwait(false);

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
                user = await AuthorisationService.UpdateUserAsync(user).ConfigureAwait(false);

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

            await AuthorisationService.DeleteUserAsync(user.UserId).ConfigureAwait(false);

            alert = new Alert
            {
                AlertType = "primary",
                Title = $"{user.UserName}",
                Message = $"has been deleted.",
                RedirectText = "Return to users.",
                RedirectPage = "/users"
            };

            IsDeleteInProgress = false;
        }

        protected void RolesCheckboxClicked(int roleId, object checkedValue)
        {

        }

        protected void PermissionCheckboxClicked(int permissionId, object checkedValue)
        {
            var permission = headwayPermissions.Single(p => p.Permission.PermissionId.Equals(permissionId));
            var userPermission = user.Permissions.SingleOrDefault(p => p.PermissionId.Equals(permissionId));

            bool isChecked = (bool)checkedValue;

            if(isChecked
                && userPermission == null)
            {
                user.Permissions.Add(permission.Permission);
            }
            else if(!isChecked
                && userPermission != null)
            {
                user.Permissions.Remove(userPermission);
            }
        }
    }
}
