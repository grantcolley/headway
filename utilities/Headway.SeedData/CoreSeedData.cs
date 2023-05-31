﻿using Headway.Blazor.Controls.Components;
using Headway.Core.Constants;
using Headway.Core.Model;
using Headway.Core.Options;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using RemediatR.Core.Constants;
using System.Collections.Generic;

namespace Headway.SeedData
{
    public class CoreSeedData
    {
        private static ApplicationDbContext dbContext;

        private static readonly Dictionary<string, Permission> permissions = new();
        private static readonly Dictionary<string, Role> roles = new();
        private static readonly Dictionary<string, User> users = new();

        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            dbContext = applicationDbContext;

            TruncateTables();

            CreatePermissions();
            CreateRoles();
            CreateUsers();
            AssignUsersRoles();

            Navigation();

            PermissionsConfig();
            PermissionConfig();

            RolesConfig();
            RoleConfig();

            UsersConfig();
            UserConfig();

            ModulesConfig();
            ModuleConfig();
            CategoriesConfig();
            CategoryConfig();
            MenuItemsConfig();
            MenuItemConfig();

            ConfigsConfig();
            ConfigConfig();
            ConfigItemConfig();
            ConfigItemsListDetailConfig();
            ConfigSearchItemConfig();
            ConfigSearchItemsListDetailConfig();
            ConfigContainerConfig();

            FlowsConfig();
            FlowConfig();
            StateConfig();
            StatesListDetailConfig();
        }

        private static void TruncateTables()
        {
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE Audits");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE Logs");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE RoleUser");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionUser");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionRole");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Users");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Users, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Roles");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Roles, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Permissions");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Permissions, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE MenuItems");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Categories");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Categories, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Modules");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Modules, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE ConfigItems");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE ConfigSearchItems");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM ConfigContainers");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (ConfigContainers, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Configs");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Configs, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE States");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Flows");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Flows, RESEED, 1)");
        }

        private static void CreatePermissions()
        {
            permissions.Add(HeadwayAuthorisation.USER, new Permission { Name = HeadwayAuthorisation.USER, Description = "Headway User" });
            permissions.Add(HeadwayAuthorisation.ADMIN, new Permission { Name = HeadwayAuthorisation.ADMIN, Description = "Headway Administrator" });

            foreach(var permission in permissions.Values)
            {
                dbContext.Permissions.Add(permission);
            }

            dbContext.SaveChanges();
        }

        private static void CreateRoles()
        {
            roles.Add(HeadwayAuthorisation.USER, new Role { Name = HeadwayAuthorisation.USER, Description = "Headway User" });
            roles.Add(HeadwayAuthorisation.ADMIN, new Role { Name = HeadwayAuthorisation.ADMIN, Description = "Headway Administrator" });

            foreach (var role in roles.Values)
            {
                dbContext.Roles.Add(role);
            }

            roles[HeadwayAuthorisation.USER].Permissions.Add(permissions[HeadwayAuthorisation.USER]);

            roles[HeadwayAuthorisation.ADMIN].Permissions.Add(permissions[HeadwayAuthorisation.USER]);
            roles[HeadwayAuthorisation.ADMIN].Permissions.Add(permissions[HeadwayAuthorisation.ADMIN]);

            dbContext.SaveChanges();
        }

        private static void CreateUsers()
        {
            users.Add("alice", new User { UserName = "alice", Email = "alice@email.com" });

            foreach(var user in users.Values)
            {
                dbContext.Users.Add(user);
            }

            dbContext.SaveChanges();
        }

        private static void AssignUsersRoles()
        {
            users["alice"].Roles.Add(roles[HeadwayAuthorisation.ADMIN]);

            dbContext.SaveChanges();
        }

        private static void Navigation()
        {
            var administration = new Module { Name = "Administration", Icon = "Engineering", Order = 2, Permission = HeadwayAuthorisation.ADMIN };

            dbContext.Modules.Add(administration);

            var authorisationCatgory = new Category { Name = "Authorisation", Icon = "AdminPanelSettings", Order = 1, Permission = HeadwayAuthorisation.ADMIN };
            var navigationCatgory = new Category { Name = "Navigation", Icon = "Explore", Order = 2, Permission = HeadwayAuthorisation.ADMIN };
            var configurationCategory = new Category { Name = "Configuration", Icon = "AppSettingsAlt", Order = 3, Permission = HeadwayAuthorisation.ADMIN };
            var flowsCategory = new Category { Name = "Flows", Icon = "AltRoute", Order = 4, Permission = HeadwayAuthorisation.ADMIN };
            var developerToolsCategory = new Category { Name = "Developer Tools", Icon = "Build", Order = 5, Permission = HeadwayAuthorisation.DEVELOPER };

            dbContext.Categories.Add(authorisationCatgory);
            dbContext.Categories.Add(configurationCategory);
            dbContext.Categories.Add(flowsCategory);
            dbContext.Categories.Add(developerToolsCategory);

            var usersMenuItem = new MenuItem { Name = "Users", Icon = "SupervisedUserCircle", NavigatePage = "Page", Order = 1, Permission = HeadwayAuthorisation.ADMIN, Config = "Users" };
            var rolesMenuItem = new MenuItem { Name = "Roles", Icon = "Lock", NavigatePage = "Page", Order = 2, Permission = HeadwayAuthorisation.ADMIN, Config = "Roles" };
            var permissionsMenuItem = new MenuItem { Name = "Permissions", Icon = "Key", NavigatePage = "Page", Order = 3, Permission = HeadwayAuthorisation.ADMIN, Config = "Permissions" };

            var modulesMenuItem = new MenuItem { Name = "Modules", Icon = "AutoAwesomeMosaic", NavigatePage = "Page", Order = 1, Permission = HeadwayAuthorisation.ADMIN, Config = "Modules" };
            var categoriesMenuItem = new MenuItem { Name = "Categories", Icon = "AutoAwesomeMotion", NavigatePage = "Page", Order = 2, Permission = HeadwayAuthorisation.ADMIN, Config = "Categories" };
            var menuItemsMenuItem = new MenuItem { Name = "MenuItems", Icon = "Article", NavigatePage = "Page", Order = 3, Permission = HeadwayAuthorisation.ADMIN, Config = "MenuItems" };

            var configureMenuItem = new MenuItem { Name = "Configure", Icon = "Settings", NavigatePage = "Page", Order = 1, Permission = HeadwayAuthorisation.ADMIN, Config = "Configs" };
            var flowMenuItem = new MenuItem { Name = "Flows", Icon = "LinearScale", NavigatePage = "Page", Order = 1, Permission = HeadwayAuthorisation.ADMIN, Config = "Flows" };            
            var demoMenuItem = new MenuItem { Name = "Demo", Icon = "Info", NavigatePage = "Page", Order = 1, Permission = HeadwayAuthorisation.DEVELOPER, Config = "DemoModels" };

            dbContext.MenuItems.Add(usersMenuItem);
            dbContext.MenuItems.Add(rolesMenuItem);
            dbContext.MenuItems.Add(permissionsMenuItem);
            dbContext.MenuItems.Add(modulesMenuItem);
            dbContext.MenuItems.Add(categoriesMenuItem);
            dbContext.MenuItems.Add(menuItemsMenuItem);
            dbContext.MenuItems.Add(configureMenuItem);
            dbContext.MenuItems.Add(flowMenuItem);
            dbContext.MenuItems.Add(demoMenuItem);

            authorisationCatgory.MenuItems.Add(usersMenuItem);
            authorisationCatgory.MenuItems.Add(rolesMenuItem);
            authorisationCatgory.MenuItems.Add(permissionsMenuItem);
            navigationCatgory.MenuItems.Add(modulesMenuItem);
            navigationCatgory.MenuItems.Add(categoriesMenuItem);
            navigationCatgory.MenuItems.Add(menuItemsMenuItem);
            configurationCategory.MenuItems.Add(configureMenuItem);
            flowsCategory.MenuItems.Add(flowMenuItem);
            developerToolsCategory.MenuItems.Add(demoMenuItem);

            administration.Categories.Add(authorisationCatgory);
            administration.Categories.Add(navigationCatgory);
            administration.Categories.Add(configurationCategory);
            administration.Categories.Add(flowsCategory);
            administration.Categories.Add(developerToolsCategory);

            dbContext.SaveChanges();
        }

        private static void PermissionsConfig()
        {
            var permissionsConfig = new Config
            {
                Name = "Permissions",
                Title = "Permissions",
                Description = "List of User Permissions",
                Model = "Headway.Core.Model.Permission, Headway.Core",
                ModelApi = "Permissions",
                OrderModelBy = "Name",
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Buttons.HEADER_BTN_IMAGE};Value={Buttons.BTN_ICON_ADD}|Name={Buttons.HEADER_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_CREATE}|Name={Buttons.ROW_BTN_IMAGE};Value={Buttons.BTN_ICON_EDIT_NOTE}|Name={Buttons.ROW_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_EDIT}",
                NavigatePage = "Page",
                NavigateProperty = "PermissionId",
                NavigateConfig = "Permission",
                NavigateResetBreadcrumb = true
            };

            dbContext.Configs.Add(permissionsConfig);

            permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", Order = 1 });
            permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3 });

            dbContext.SaveChanges();
        }

        private static void PermissionConfig()
        {
            var permissionConfig = new Config
            {
                Name = "Permission",
                Title = "Permission",
                Description = "Create, update or delete a User Permission",
                Model = "Headway.Core.Model.Permission, Headway.Core",
                ModelApi = "Permissions",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.TabDocument`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Permissions"
            };

            dbContext.Configs.Add(permissionConfig);

            var permissionConfigContainer = new ConfigContainer { Name = "Permission Div", Code = "PERMISSION DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Permission", Order = 1 };
            var membershipConfigContainer = new ConfigContainer { Name = "Permission Membership Div", Code = "PERMISSION MEMBERSHIP DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Membership", Order = 2, ComponentArgs = "Name=LayoutHorizontal;Value=True" };

            permissionConfig.ConfigContainers.Add(permissionConfigContainer);
            permissionConfig.ConfigContainers.Add(membershipConfigContainer);

            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", IsIdentity = true, Order = 1, ConfigContainer = permissionConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = permissionConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3, ConfigContainer = permissionConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RoleList", Label = "Roles", Order = 5, ConfigContainer = membershipConfigContainer, Component = "Headway.Blazor.Controls.Components.StringList, Headway.Blazor.Controls" });
            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UserList", Label = "Users", Order = 6, ConfigContainer = membershipConfigContainer, Component = "Headway.Blazor.Controls.Components.StringList, Headway.Blazor.Controls" });

            dbContext.SaveChanges();
        }

        private static void RolesConfig()
        {
            var rolesConfig = new Config
            {
                Name = "Roles",
                Title = "Roles",
                Description = "List of User Roles",
                Model = "Headway.Core.Model.Role, Headway.Core",
                ModelApi = "Roles",
                OrderModelBy = "Name",
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Buttons.HEADER_BTN_IMAGE};Value={Buttons.BTN_ICON_ADD}|Name={Buttons.HEADER_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_CREATE}|Name={Buttons.ROW_BTN_IMAGE};Value={Buttons.BTN_ICON_EDIT_NOTE}|Name={Buttons.ROW_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_EDIT}",
                NavigatePage = "Page",
                NavigateProperty = "RoleId",
                NavigateConfig = "Role",
                NavigateResetBreadcrumb = true
            };

            dbContext.Configs.Add(rolesConfig);

            rolesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RoleId", Label = "Role Id", Order = 1 });
            rolesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            rolesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3 });

            dbContext.SaveChanges();
        }

        private static void RoleConfig()
        {
            var roleConfig = new Config
            {
                Name = "Role",
                Title = "Role",
                Description = "Create, update or delete a User Role",
                Model = "Headway.Core.Model.Role, Headway.Core",
                ModelApi = "Roles",
                CreateLocal = false,
                Document = "Headway.Blazor.Controls.Documents.TabDocument`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Roles"
            };

            dbContext.Configs.Add(roleConfig);

            var roleConfigContainer = new ConfigContainer { Name = "Role Div", Code = "ROLE DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Role", Order = 1 };
            var authConfigContainer = new ConfigContainer { Name = "Role Auth Div", Code = "ROLE AUTH DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Authorisation", Order = 2, ComponentArgs = "Name=LayoutHorizontal;Value=True" };
            var memberConfigContainer = new ConfigContainer { Name = "Role Membership Div", Code = "ROLE MEMBERSHIP DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Membership", Order = 3, ComponentArgs = "Name=LayoutHorizontal;Value=True" };

            roleConfig.ConfigContainers.Add(roleConfigContainer);
            roleConfig.ConfigContainers.Add(authConfigContainer);
            roleConfig.ConfigContainers.Add(memberConfigContainer);

            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RoleId", Label = "Role Id", IsIdentity = true, Order = 1, ConfigContainer = roleConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = roleConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3, ConfigContainer = roleConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionChecklist", Label = "Permissions", Order = 4, ConfigContainer = authConfigContainer, Component = "Headway.Blazor.Controls.Components.CheckList, Headway.Blazor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionList", Label = "Permissions", Order = 5, ConfigContainer = memberConfigContainer, Component = "Headway.Blazor.Controls.Components.StringList, Headway.Blazor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UserList", Label = "Users", Order = 6, ConfigContainer = memberConfigContainer, Component = "Headway.Blazor.Controls.Components.StringList, Headway.Blazor.Controls" });

            dbContext.SaveChanges();
        }

        private static void UsersConfig()
        {
            var usersConfig = new Config
            {
                Name = "Users",
                Title = "Users",
                Description = "List of Users",
                Model = "Headway.Core.Model.User, Headway.Core",
                ModelApi = "Users",
                OrderModelBy = "UserName",
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Buttons.HEADER_BTN_IMAGE};Value={Buttons.BTN_ICON_ADD}|Name={Buttons.HEADER_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_CREATE}|Name={Buttons.ROW_BTN_IMAGE};Value={Buttons.BTN_ICON_EDIT_NOTE}|Name={Buttons.ROW_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_EDIT}",
                NavigatePage = "Page",
                NavigateProperty = "UserId",
                NavigateConfig = "User",
                NavigateResetBreadcrumb = true
            };

            dbContext.Configs.Add(usersConfig);

            usersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UserId", Label = "User Id", Order = 1 });
            usersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UserName", Label = "User Name", Order = 2 });
            usersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Email", Label = "Email", Order = 3 });

            dbContext.SaveChanges();
        }

        private static void UserConfig()
        {
            var userConfig = new Config
            {
                Name = "User",
                Title = "User",
                Description = "Create, update or delete a User",
                Model = "Headway.Core.Model.User, Headway.Core",
                ModelApi = "Users",
                CreateLocal = false,
                Document = "Headway.Blazor.Controls.Documents.TabDocument`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Users"
            };

            dbContext.Configs.Add(userConfig);

            var userConfigContainer = new ConfigContainer { Name = "User Div", Code = "USER DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "User", Order = 1 };
            var authConfigContainer = new ConfigContainer { Name = "User Auth Div", Code = "USER AUTH DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Authorisation", Order = 2, ComponentArgs = "Name=LayoutHorizontal;Value=True" };
            var membConfigContainer = new ConfigContainer { Name = "User Membership Div", Code = "USER MEMBERSHIP DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Membership", Order = 3, ComponentArgs = "Name=LayoutHorizontal;Value=True" };

            userConfig.ConfigContainers.Add(userConfigContainer);
            userConfig.ConfigContainers.Add(authConfigContainer);
            userConfig.ConfigContainers.Add(membConfigContainer);

            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UserId", Label = "User Id", IsIdentity = true, Order = 1, ConfigContainer = userConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UserName", Label = "User Name", IsTitle = true, Order = 2, ConfigContainer = userConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Email", Label = "Email", Order = 3, ConfigContainer = userConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RoleChecklist", Label = "Roles", Order = 4, ConfigContainer = authConfigContainer, Component = "Headway.Blazor.Controls.Components.CheckList, Headway.Blazor.Controls" });
            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionChecklist", Label = "Permissions", Order = 5, ConfigContainer = authConfigContainer, Component = "Headway.Blazor.Controls.Components.CheckList, Headway.Blazor.Controls" });
            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RoleList", Label = "Roles", Order = 6, ConfigContainer = membConfigContainer, Component = "Headway.Blazor.Controls.Components.StringList, Headway.Blazor.Controls" });
            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionList", Label = "Permissions", Order = 7, ConfigContainer = membConfigContainer, Component = "Headway.Blazor.Controls.Components.StringList, Headway.Blazor.Controls" });

            dbContext.SaveChanges();
        }

        private static void ModulesConfig()
        {
            var modulesConfig = new Config
            {
                Name = "Modules",
                Title = "Modules",
                Description = "List of navigation Modules",
                Model = "Headway.Core.Model.Module, Headway.Core",
                ModelApi = "Modules",
                OrderModelBy = "Name",
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Buttons.HEADER_BTN_IMAGE};Value={Buttons.BTN_ICON_ADD}|Name={Buttons.HEADER_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_CREATE}|Name={Buttons.ROW_BTN_IMAGE};Value={Buttons.BTN_ICON_EDIT_NOTE}|Name={Buttons.ROW_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_EDIT}",
                NavigatePage = "Page",
                NavigateProperty = "ModuleId",
                NavigateConfig = "Module",
                NavigateResetBreadcrumb = true
            };

            dbContext.Configs.Add(modulesConfig);

            modulesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ModuleId", Label = "Module Id", Order = 1 });
            modulesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            modulesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Permission", Label = "Permission", Order = 3 });

            dbContext.SaveChanges();
        }

        private static void ModuleConfig()
        {
            var moduleConfig = new Config
            {
                Name = "Module",
                Title = "Module",
                Description = "Create, update or delete a navigation Module",
                Model = "Headway.Core.Model.Module, Headway.Core",
                ModelApi = "Modules",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.Document`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Modules"
            };

            dbContext.Configs.Add(moduleConfig);

            var moduleConfigContainer = new ConfigContainer { Name = "Module Div", Code = "MODULE_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Module", Order = 1 };

            moduleConfig.ConfigContainers.Add(moduleConfigContainer);

            moduleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ModuleId", Label = "Module Id", IsIdentity = true, Order = 1, ConfigContainer = moduleConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            moduleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = moduleConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            moduleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 3, ConfigContainer = moduleConfigContainer, Component = "Headway.Blazor.Controls.Components.Integer, Headway.Blazor.Controls" });
            moduleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Icon", Label = "Icon", Order = 4, ConfigContainer = moduleConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            moduleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Permission", Label = "Permission", Order = 5, ConfigContainer = moduleConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.PERMISSIONS_OPTION_ITEMS}" });

            dbContext.SaveChanges();
        }

        private static void CategoriesConfig()
        {
            var categoriesConfig = new Config
            {
                Name = "Categories",
                Title = "Categories",
                Description = "List of Categories for a Module",
                Model = "Headway.Core.Model.Category, Headway.Core",
                ModelApi = "Categories",
                OrderModelBy = "Name",
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Buttons.HEADER_BTN_IMAGE};Value={Buttons.BTN_ICON_ADD}|Name={Buttons.HEADER_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_CREATE}|Name={Buttons.ROW_BTN_IMAGE};Value={Buttons.BTN_ICON_EDIT_NOTE}|Name={Buttons.ROW_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_EDIT}",
                NavigatePage = "Page",
                NavigateProperty = "CategoryId",
                NavigateConfig = "Category",
                NavigateResetBreadcrumb = true
            };

            dbContext.Configs.Add(categoriesConfig);

            categoriesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CategoryId", Label = "Category Id", Order = 1 });
            categoriesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            categoriesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Permission", Label = "Permission", Order = 3 });

            dbContext.SaveChanges();
        }

        private static void CategoryConfig()
        {
            var categoryConfig = new Config
            {
                Name = "Category",
                Title = "Category",
                Description = "Create, update or delete a Category for a Module",
                Model = "Headway.Core.Model.Category, Headway.Core",
                ModelApi = "Categories",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.Document`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Categories"
            };

            dbContext.Configs.Add(categoryConfig);

            var categoryConfigContainer = new ConfigContainer { Name = "Category Div", Code = "CATEGORY_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Category", Order = 1 };

            categoryConfig.ConfigContainers.Add(categoryConfigContainer);

            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CategoryId", Label = "Category Id", IsIdentity = true, Order = 1, ConfigContainer = categoryConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = categoryConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 3, ConfigContainer = categoryConfigContainer, Component = "Headway.Blazor.Controls.Components.Integer, Headway.Blazor.Controls" });
            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Icon", Label = "Icon", Order = 4, ConfigContainer = categoryConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Permission", Label = "Permission", Order = 5, ConfigContainer = categoryConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.PERMISSIONS_OPTION_ITEMS}" });
            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Module", Label = "Module", Order = 6, ConfigContainer = categoryConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.MODULES_OPTION_ITEMS}|Name={Options.DISPLAY_FIELD};Value=Name|Name={Args.MODEL};Value=Headway.Core.Model.Module, Headway.Core|Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownComplex`1, Headway.Blazor.Controls" });

            dbContext.SaveChanges();
        }

        private static void MenuItemsConfig()
        {
            var menuItemsConfig = new Config
            {
                Name = "MenuItems",
                Title = "Menu Items",
                Description = "List of Menu Items for a Category",
                Model = "Headway.Core.Model.MenuItem, Headway.Core",
                ModelApi = "MenuItems",
                OrderModelBy = "Name",
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Buttons.HEADER_BTN_IMAGE};Value={Buttons.BTN_ICON_ADD}|Name={Buttons.HEADER_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_CREATE}|Name={Buttons.ROW_BTN_IMAGE};Value={Buttons.BTN_ICON_EDIT_NOTE}|Name={Buttons.ROW_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_EDIT}",
                NavigatePage = "Page",
                NavigateProperty = "MenuItemId",
                NavigateConfig = "MenuItem",
                NavigateResetBreadcrumb = true
            };

            dbContext.Configs.Add(menuItemsConfig);

            menuItemsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "MenuItemId", Label = "Menu Item Id", Order = 1 });
            menuItemsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            menuItemsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Permission", Label = "Permission", Order = 3 });

            dbContext.SaveChanges();
        }

        private static void MenuItemConfig()
        {
            var menuItemConfig = new Config
            {
                Name = "MenuItem",
                Title = "Menu Item",
                Description = "Create, update or delete a Menu Item for a Category",
                Model = "Headway.Core.Model.MenuItem, Headway.Core",
                ModelApi = "MenuItems",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.Document`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "MenuItems"
            };

            dbContext.Configs.Add(menuItemConfig);

            var menuItemConfigContainer = new ConfigContainer { Name = "Menu Item Div", Code = "MENUITEM_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Menu Item", Order = 1 };

            menuItemConfig.ConfigContainers.Add(menuItemConfigContainer);

            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "MenuItemId", Label = "Menu Item Id", IsIdentity = true, Order = 1, ConfigContainer = menuItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = menuItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Icon", Label = "Icon", Order = 3, ConfigContainer = menuItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Config", Label = "Config", Order = 3, ConfigContainer = menuItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONFIG_OPTION_ITEMS}" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigatePage", Label = "Navigate Page", Order = 3, ConfigContainer = menuItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(PageOptionItems)}" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 4, ConfigContainer = menuItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Integer, Headway.Blazor.Controls" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Permission", Label = "Permission", Order = 5, ConfigContainer = menuItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.PERMISSIONS_OPTION_ITEMS}" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Category", Label = "Category", Order = 6, ConfigContainer = menuItemConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CATEGORIES_OPTION_ITEMS}|Name={Options.DISPLAY_FIELD};Value=Name|Name={Args.MODEL};Value=Headway.Core.Model.Category, Headway.Core|Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownComplex`1, Headway.Blazor.Controls" });

            dbContext.SaveChanges();
        }

        private static void ConfigsConfig()
        {
            var configsConfig = new Config
            {
                Name = "Configs",
                Title = "Configs",
                Description = "List of Configs for rendering an object on a page",
                Model = "Headway.Core.Model.Config, Headway.Core",
                ModelApi = "Configuration",
                OrderModelBy = "Name",
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Buttons.HEADER_BTN_IMAGE};Value={Buttons.BTN_ICON_ADD}|Name={Buttons.HEADER_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_CREATE}|Name={Buttons.ROW_BTN_IMAGE};Value={Buttons.BTN_ICON_EDIT_NOTE}|Name={Buttons.ROW_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_EDIT}",
                NavigatePage = "Page",
                NavigateProperty = "ConfigId",
                NavigateConfig = "Config",
                NavigateResetBreadcrumb = true
            };

            dbContext.Configs.Add(configsConfig);

            configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigId", Label = "Config Id", Order = 1 });
            configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3 });

            dbContext.SaveChanges();
        }

        private static void ConfigConfig()
        {
            var configConfig = new Config
            {
                Name = "Config",
                Title = "Config",
                Description = "Config for rendering a custom object on a page",
                Model = "Headway.Core.Model.Config, Headway.Core",
                ModelApi = "Configuration",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.TabDocument`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Configs"
            };

            dbContext.Configs.Add(configConfig);

            var configConfigContainer1 = new ConfigContainer { Name = "Model Div", Code = "MODEL DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Model", Order = 1 };
            var configConfigContainer2 = new ConfigContainer { Name = "Fields Div", Code = "FIELDS_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Fields", Order = 2 };
            var configConfigContainer3 = new ConfigContainer { Name = "Search Div", Code = "SEARCH_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Search", Order = 3 };
            var configConfigContainer4 = new ConfigContainer { Name = "Containers Div", Code = "CONTAINERS_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Containers", Order = 4 };

            configConfig.ConfigContainers.Add(configConfigContainer1);
            configConfig.ConfigContainers.Add(configConfigContainer2);
            configConfig.ConfigContainers.Add(configConfigContainer3);
            configConfig.ConfigContainers.Add(configConfigContainer4);

            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigId", Label = "Config Id", IsIdentity = true, Order = 1, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", IsTitle = true, Order = 3, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", IsTitle = false, Order = 4, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Model", Label = "Model", Order = 5, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelOptionItems)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ModelApi", Label = "Model Api", Order = 6, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONTROLLER_OPTION_ITEMS}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Flow", Label = "Flow", Order = 7, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.FLOWS_COMPLEX_OPTION_ITEMS}|Name={Options.DISPLAY_FIELD};Value=Name|Name={Args.MODEL};Value=Headway.Core.Model.Flow, Headway.Core|Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownComplex`1, Headway.Blazor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CreateLocal", Label = "Create Local", Order = 8, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Checkbox, Headway.Blazor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UseSearchComponent", Label = "UseSearch Component", Order = 9, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Checkbox, Headway.Blazor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "SearchComponent", Label = "Search Component", Order = 10, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(SearchComponentOptionItems)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OrderModelBy", Label = "Order Model By", Order = 11, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelFieldsOptionItems)}|Name={Args.LINK_SOURCE};Value=Model" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Document", Label = "Document", Order = 12, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(DocumentOptionItems)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DocumentArgs", Label = "DocumentArgs", Order = 13, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.TextMultiline, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.TEXT_MULTILINE_ROWS};Value=3" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigatePage", Label = "Navigate Page", Order = 14, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(PageOptionItems)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateResetBreadcrumb", Label = "Navigate Reset Breadcrumb", Order = 15, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Checkbox, Headway.Blazor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateProperty", Label = "Navigate Property", Order = 16, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelFieldsOptionItems)}|Name={Args.LINK_SOURCE};Value=Model" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateConfig", Label = "Navigate Config", Order = 17, ConfigContainer = configConfigContainer1, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONFIG_OPTION_ITEMS}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigItems", Label = "Config Items", Order = 18, ConfigContainer = configConfigContainer2, Component = "Headway.Blazor.Controls.Components.GenericField, Headway.Blazor.Controls", ConfigName = "ConfigItem", ComponentArgs = $"Name={Args.LIST_CONFIG};Value=ConfigItemsListDetail|Name={Args.PROPAGATE_FIELDS};Value=Model,ConfigId" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigSearchItems", Label = "Config Search Items", Order = 19, ConfigContainer = configConfigContainer3, Component = "Headway.Blazor.Controls.Components.GenericField, Headway.Blazor.Controls", ConfigName = "ConfigSearchItem", ComponentArgs = $"Name={Args.LIST_CONFIG};Value=ConfigSearchItemsListDetail" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainers", Label = "Config Containers", Tooltip = "Drag and drop containers into nested hierarchy", Order = 20, ConfigContainer = configConfigContainer4, Component = "Headway.Blazor.Controls.Components.GenericField, Headway.Blazor.Controls", ConfigName = "ConfigContainer", ComponentArgs = $"Name={Args.UNIQUE_PROPERTY};Value={Args.CODE}|Name={Args.UNIQUE_PARENT_PROPERTY};Value={Args.CODE_PARENT}|Name={Args.LABEL_PROPERTY};Value=Label|Name={Args.LIST_PROPERTY};Value=ConfigContainers" });

            dbContext.SaveChanges();
        }

        private static void ConfigItemConfig()
        {
            var configItemConfig = new Config
            {
                Name = "ConfigItem",
                Title = "ConfigItem",
                Description = "Config Item maps to a custom object's property to be rendered on the screen",
                Model = "Headway.Core.Model.ConfigItem, Headway.Core",
                ModelApi = "Configuration",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.ListDetail`1, Headway.Blazor.Controls"
            };

            dbContext.Configs.Add(configItemConfig);

            var configItemConfigContainer = new ConfigContainer { Name = "Config Item Div", Code = "CONFIG_ITEM_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Config Item", Order = 1 };

            configItemConfig.ConfigContainers.Add(configItemConfigContainer);

            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigItemId", Label = "Config Item Id", IsIdentity = true, Order = 1, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PropertyName", Label = "PropertyName", Order = 2, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelFieldsOptionItems)}|Name={Args.LINK_SOURCE};Value=Model" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Label", Label = "Label", Order = 3, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Tooltip", Label = "Tooltip", Order = 4, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "IsIdentity", Label = "IsIdentity", Order = 5, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Checkbox, Headway.Blazor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "IsTitle", Label = "IsTitle", Order = 6, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Checkbox, Headway.Blazor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 7, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Integer, Headway.Blazor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Component", Label = "Component", Order = 8, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ComponentOptionItems)}" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ComponentArgs", Label = "ComponentArgs", Order = 9, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.TextMultiline, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.TEXT_MULTILINE_ROWS};Value=3" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigName", Label = "Config Name", Order = 10, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONFIG_OPTION_ITEMS}" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainer", Label = "Container", Order = 11, ConfigContainer = configItemConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONFIG_CONTAINERS}|Name={Args.LINK_SOURCE};VALUE=ConfigId|Name={Options.DISPLAY_FIELD};Value=Name|Name={Args.MODEL};Value=Headway.Core.Model.ConfigContainer, Headway.Core|Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownComplex`1, Headway.Blazor.Controls" });

            dbContext.SaveChanges();
        }

        private static void ConfigItemsListDetailConfig()
        {
            var configItemsListDetailConfig = new Config
            {
                Name = "ConfigItemsListDetail",
                Title = "ConfigItemsListDetail",
                Description = "List of custom object properties to be rendered on the screen",
                Model = "Headway.Core.Model.ConfigItem, Headway.Core",
                ModelApi = "Configuration",
                OrderModelBy = "Order"
            };

            dbContext.Configs.Add(configItemsListDetailConfig);

            configItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PropertyName", Label = "Property Name", Order = 1 });
            configItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 2 });

            dbContext.SaveChanges();
        }

        private static void ConfigSearchItemConfig()
        {
            var configSearchItemConfig = new Config
            {
                Name = "ConfigSearchItem",
                Title = "ConfigSearchItem",
                Description = "Config Search Item configures a dynamic search component",
                Model = "Headway.Core.Model.ConfigSearchItem, Headway.Core",
                ModelApi = "Configuration",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.ListDetail`1, Headway.Blazor.Controls"
            };

            dbContext.Configs.Add(configSearchItemConfig);

            var configSearchItemConfigContainer = new ConfigContainer { Name = "Config Search Item Div", Code = "CONFIG_SEARCH_ITEM_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Config Search Item", Order = 1 };

            configSearchItemConfig.ConfigContainers.Add(configSearchItemConfigContainer);

            configSearchItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigSearchItemId", Label = "Config Search Item Id", IsIdentity = true, Order = 1, ConfigContainer = configSearchItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            configSearchItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Label", Label = "Label", Order = 2, ConfigContainer = configSearchItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configSearchItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ParameterName", Label = "Parameter Name", Order = 3, ConfigContainer = configSearchItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configSearchItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Tooltip", Label = "Tooltip", Order = 4, ConfigContainer = configSearchItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configSearchItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 5, ConfigContainer = configSearchItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Integer, Headway.Blazor.Controls" });
            configSearchItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Component", Label = "Component", Order = 6, ConfigContainer = configSearchItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(SearchItemComponentOptionItems)}" });
            configSearchItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ComponentArgs", Label = "Component Args", Order = 7, ConfigContainer = configSearchItemConfigContainer, Component = "Headway.Blazor.Controls.Components.TextMultiline, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.TEXT_MULTILINE_ROWS};Value=3" });

            dbContext.SaveChanges();
        }

        private static void ConfigSearchItemsListDetailConfig()
        {
            var configSearchItemsListDetailConfig = new Config
            {
                Name = "ConfigSearchItemsListDetail",
                Title = "ConfigSearchItemsListDetail",
                Description = "List of config search items for configuring a dynamic search component",
                Model = "Headway.Core.Model.ConfigSearchItem, Headway.Core",
                ModelApi = "Configuration",
                OrderModelBy = "Order"
            };

            dbContext.Configs.Add(configSearchItemsListDetailConfig);

            configSearchItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Label", Label = "Label", Order = 1 });
            configSearchItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 2 });

            dbContext.SaveChanges();
        }

        private static void ConfigContainerConfig()
        {
            var configContainerConfig = new Config
            {
                Name = "ConfigContainer",
                Title = "ConfigContainer",
                Description = "Container for rendering Config Items for a Config",
                Model = "Headway.Core.Model.ConfigContainer, Headway.Core",
                ModelApi = "Configuration",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.TreeDetail`1, Headway.Blazor.Controls"
            };

            dbContext.Configs.Add(configContainerConfig);

            var configContainerConfigContainer = new ConfigContainer { Name = "Config Container Div", Code = "CONFIG_CONTAINER_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Config Container", Order = 1 };

            configContainerConfig.ConfigContainers.Add(configContainerConfigContainer);

            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainerId", Label = "Config Container Id", IsIdentity = true, Order = 1, ConfigContainer = configContainerConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Code", Label = "Code", IsIdentity = true, Order = 2, ConfigContainer = configContainerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ParentCode", Label = "Parent Code", IsIdentity = true, Order = 3, ConfigContainer = configContainerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 4, ConfigContainer = configContainerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Label", Label = "Label", Order = 5, ConfigContainer = configContainerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ComponentArgs", Label = "Component Args", Order = 6, ConfigContainer = configContainerConfigContainer, Component = "Headway.Blazor.Controls.Components.TextMultiline, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.TEXT_MULTILINE_ROWS};Value=3" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FlowArgs", Label = "Flow Args", Order = 7, ConfigContainer = configContainerConfigContainer, Component = "Headway.Blazor.Controls.Components.TextMultiline, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.TEXT_MULTILINE_ROWS};Value=3" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 8, ConfigContainer = configContainerConfigContainer, Component = "Headway.Blazor.Controls.Components.Integer, Headway.Blazor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Container", Label = "Container", Order = 9, ConfigContainer = configContainerConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ContainerOptionItems)}" });
            dbContext.SaveChanges();
        }

        private static void FlowsConfig()
        {
            var flowsConfig = new Config
            {
                Name = "Flows",
                Title = "Flows",
                Description = "List of Flows",
                Model = "Headway.Core.Model.Flow, Headway.Core",
                ModelApi = "Flow",
                OrderModelBy = "Name",
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Buttons.HEADER_BTN_IMAGE};Value={Buttons.BTN_ICON_ADD}|Name={Buttons.HEADER_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_CREATE}|Name={Buttons.ROW_BTN_IMAGE};Value={Buttons.BTN_ICON_EDIT_NOTE}|Name={Buttons.ROW_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_EDIT}",
                NavigatePage = "Page",
                NavigateProperty = "FlowId",
                NavigateConfig = "Flow",
                NavigateResetBreadcrumb = true
            };

            dbContext.Configs.Add(flowsConfig);

            flowsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FlowId", Label = "Flow Id", Order = 1 });
            flowsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });

            dbContext.SaveChanges();
        }

        private static void FlowConfig()
        {
            var flowConfig = new Config
            {
                Name = "Flow",
                Title = "Flow",
                Description = "Create, update or delete a Flow",
                Model = "Headway.Core.Model.Flow, Headway.Core",
                ModelApi = "Flow",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.TabDocument`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Flows"
            };

            dbContext.Configs.Add(flowConfig);

            var flowConfigContainer = new ConfigContainer { Name = "Flow Div", Code = "FLOW_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Flow", Order = 1 };
            var statesContainer = new ConfigContainer { Name = "States Div", Code = "STATES_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "States", Order = 2 };

            flowConfig.ConfigContainers.Add(flowConfigContainer);
            flowConfig.ConfigContainers.Add(statesContainer);

            flowConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FlowId", Label = "Flow Id", IsIdentity = true, Order = 1, ConfigContainer = flowConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            flowConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = flowConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            flowConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FlowCode", Label = "Flow Code", IsTitle = true, Order = 3, ConfigContainer = flowConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            flowConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Permission", Label = "Permission", Order = 4, ConfigContainer = flowConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.PERMISSIONS_OPTION_ITEMS}" });
            flowConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FlowConfigurationClass", Label = "Flow Configuration Class", Order = 5, ConfigContainer = flowConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(FlowConfigurationOptionItems)}" });
            flowConfig.ConfigItems.Add(new ConfigItem { PropertyName = "States", Label = "States", Order = 6, ConfigContainer = statesContainer, Component = "Headway.Blazor.Controls.Components.GenericField, Headway.Blazor.Controls", ConfigName = "State", ComponentArgs = $"Name={Args.LIST_CONFIG};Value=StatesListDetail" });

            dbContext.SaveChanges();
        }

        private static void StateConfig()
        {
            var stateConfig = new Config
            {
                Name = "State",
                Title = "State",
                Description = "A state represents a step in a flow.",
                Model = "Headway.Core.Model.State, Headway.Core",
                ModelApi = "Flow",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.ListDetail`1, Headway.Blazor.Controls"
            };

            dbContext.Configs.Add(stateConfig);

            var stateConfigContainer = new ConfigContainer { Name = "State Div", Code = "STATE_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "State", Order = 1 };

            stateConfig.ConfigContainers.Add(stateConfigContainer);

            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StateId", Label = "State Id", IsIdentity = true, Order = 1, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StateCode", Label = "State Code", Order = 3, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ParentStateCode", Label = "Parent State Code", Order = 4, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StateType", Label = "State Type", Order = 5, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownEnum`1, Headway.Blazor.Controls|Name={Args.MODEL};Value=Headway.Core.Enums.StateType, Headway.Core" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Position", Label = "Position", Order = 6, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Integer, Headway.Blazor.Controls" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "IsOwnerRestricted", Label = "Is Owner Restricted", Order = 7, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Checkbox, Headway.Blazor.Controls" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ReadPermission", Label = "Read Permission", Order = 8, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.PERMISSIONS_OPTION_ITEMS}" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "WritePermission", Label = "Write Permission", Order = 9, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.PERMISSIONS_OPTION_ITEMS}" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "SubStateCodes", Label = "Sub State Codes", Order = 10, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "TransitionStateCodes", Label = "Transition State Codes", Order = 11, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RegressionStateCodes", Label = "Regression State Codes", Order = 12, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StateConfigurationClass", Label = "State Configuration Class", Order = 13, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ContextFullName", Label = "Context Full Name", Order = 14, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelOptionItems)}" });
            stateConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ContextProperties", Label = "Context Properties", Order = 15, ConfigContainer = stateConfigContainer, Component = "Headway.Blazor.Controls.Components.CheckListToText, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelFieldsOptionItems)}|Name={Args.LINK_SOURCE};Value=Model" });

            dbContext.SaveChanges();
        }

        private static void StatesListDetailConfig()
        {
            var statesListDetailConfig = new Config
            {
                Name = "StatesListDetail",
                Title = "StatesListDetail",
                Description = "List of states in the flow.",
                Model = "Headway.Core.Model.State, Headway.Core",
                ModelApi = "Flow",
                OrderModelBy = "Position"
            };

            dbContext.Configs.Add(statesListDetailConfig);

            statesListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 1 });
            statesListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Position", Label = "Position", Order = 2 });

            dbContext.SaveChanges();
        }
    }
}