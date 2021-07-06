using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RazorAdmin.Model;
using Headway.RazorShared.Base;
using Headway.RazorShared.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.RazorAdmin.Pages
{
    public class RoleDetailsBase : HeadwayComponentBase
    {
        [Inject]
        public IAuthorisationService AuthorisationService { get; set; }

        [Parameter]
        public int RoleId { get; set; }

        protected Role role { get; set; }

        protected List<HeadwayPermission> headwayPermissions { get; set; }

        protected Alert alert { get; set; }

        protected bool IsSaveInProgress = false;
        protected bool IsDeleteInProgress = false;

        protected override async Task OnInitializedAsync()
        {
            if (RoleId.Equals(0))
            {
                role = new Role();
            }
            else
            {
                var roleResponse = await AuthorisationService.GetRoleAsync(RoleId).ConfigureAwait(false);
                role = GetResponse(roleResponse);
                if(role == null)
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
                                  join rp in role.Permissions on p.PermissionId equals rp.PermissionId into j
                                  from res in j.DefaultIfEmpty()
                                  select new HeadwayPermission(p) { IsSelected = res == null ? false : true }).ToList();

            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        protected async Task SubmitRole()
        {
            IsSaveInProgress = true;

            if (role.RoleId.Equals(0))
            {
                var roleResponse = await AuthorisationService.AddRoleAsync(role).ConfigureAwait(false);
                role = GetResponse(roleResponse);
                if(role == null)
                {
                    return;
                }

                alert = new Alert
                {
                    AlertType = "primary",
                    Title = $"{role.Name}",
                    Message = $"has been added.",
                    RedirectText = "Return to roles.",
                    RedirectPage = "/roles"
                };
            }
            else
            {
                var roleResponse = await AuthorisationService.UpdateRoleAsync(role).ConfigureAwait(false);
                role = GetResponse(roleResponse);
                if (role == null)
                {
                    return;
                }

                alert = new Alert
                {
                    AlertType = "primary",
                    Title = $"{role.Name}",
                    Message = $"has been updated.",
                    RedirectText = "Return to roles.",
                    RedirectPage = "/roles"
                };
            }

            IsSaveInProgress = false;
        }

        public async Task DeleteRole(Role role)
        {
            IsDeleteInProgress = true;

            var deleteResponse = await AuthorisationService.DeleteRoleAsync(role.RoleId).ConfigureAwait(false);
            var deleteResult = GetResponse(deleteResponse);
            if(deleteResult.Equals(0))
            {
                return;
            }

            alert = new Alert
            {
                AlertType = "danger",
                Title = $"{role.Name}",
                Message = $"has been deleted.",
                RedirectText = "Return to roles.",
                RedirectPage = "/roles"
            };

            IsDeleteInProgress = false;
        }

        protected void PermissionCheckboxClicked(int permissionId, object checkedValue)
        {
            var permission = headwayPermissions.Single(p => p.Permission.PermissionId.Equals(permissionId));
            var rolePermission = role.Permissions.SingleOrDefault(p => p.PermissionId.Equals(permissionId));

            bool isChecked = (bool)checkedValue;

            if (isChecked
                && rolePermission == null)
            {
                role.Permissions.Add(permission.Permission);
            }
            else if (!isChecked
                && rolePermission != null)
            {
                role.Permissions.Remove(rolePermission);
            }
        }
    }
}
