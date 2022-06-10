using Headway.Core.Constants;
using Headway.Core.Model;
using Headway.Core.Options;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Headway.SeedData
{
    public class CoreData
    {
        private static ApplicationDbContext dbContext;

        private static Dictionary<string, Permission> permissions = new Dictionary<string, Permission>();
        private static Dictionary<string, Role> roles = new Dictionary<string, Role>();
        private static Dictionary<string, User> users = new Dictionary<string, User>();

        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            dbContext = applicationDbContext;

            TruncateTables();

            CreatePermissions();
            CreateRoles();
            CreateUsers();
            AssignUsersRoles();

            CreateDemoModel();

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
            ConfigContainerConfig();

            DemoModelsConfig();
            DemoModelConfig();
            DemoModelItemConfig();
            DemoModelItemsListDetailConfig();
            DemoModelTreeItemConfig();
        }

        private static void TruncateTables()
        {
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE RoleUser");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionUser");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionRole");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Users");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Users, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Roles");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Roles, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Permissions");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Permissions, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE DemoModelItems");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE DemoModelTreeItems");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM DemoModels");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (DemoModels, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM DemoModelComplexProperties");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (DemoModelComplexProperties, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE MenuItems");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Categories");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Categories, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Modules");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Modules, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE ConfigItems");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM ConfigContainers");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (ConfigContainers, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM Configs");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Configs, RESEED, 1)");
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

        private static void CreateDemoModel()
        {
            var demoModel = new DemoModel
            {
                Text = "Sample text...",
                DemoModelItems = new List<DemoModelItem>
                {
                    new DemoModelItem { Name = "Item 1", Order = 1 },
                    new DemoModelItem { Name = "Item 2", Order = 2 }
                },
                DemoModelTreeItems = new List<DemoModelTreeItem>
                {
                    new DemoModelTreeItem { Name = "Tree Item 1", Code = "TREE_ITEM_1", Order = 1 },
                    new DemoModelTreeItem { Name = "Child Item 1.1", Code = "Child_ITEM_1.1", ParentCode = "TREE_ITEM_1", Order = 1 },
                    new DemoModelTreeItem { Name = "Child Item 1.2", Code = "Child_ITEM_1.2", ParentCode = "TREE_ITEM_1", Order = 2 },
                    new DemoModelTreeItem { Name = "Tree Item 2", Code = "TREE_ITEM_2", Order = 2 },
                    new DemoModelTreeItem { Name = "Child Item 2.1", Code = "Child_ITEM_2.1", ParentCode = "TREE_ITEM_2", Order = 1 },
                    new DemoModelTreeItem { Name = "Child Item 2.2", Code = "Child_ITEM_2.2", ParentCode = "TREE_ITEM_2", Order = 2 }
                 }
            };

            dbContext.DemoModels.Add(demoModel);

            var demoModelComplexProperties = new List<DemoModelComplexProperty>
            {
                new DemoModelComplexProperty{ Name ="Complex Property 1" },
                new DemoModelComplexProperty{ Name ="Complex Property 2" },
                new DemoModelComplexProperty{ Name ="Complex Property 3" }
            };

            dbContext.DemoModelComplexProperties.AddRange(demoModelComplexProperties);

            dbContext.SaveChanges();
        }

        private static void Navigation()
        {
            var administration = new Module { Name = "Administration", Icon = "Engineering", Order = 2, Permission = HeadwayAuthorisation.ADMIN };

            dbContext.Modules.Add(administration);

            var authorisationCatgory = new Category { Name = "Authorisation", Icon = "AdminPanelSettings", Order = 1, Permission = HeadwayAuthorisation.ADMIN };
            var navigationCatgory = new Category { Name = "Navigation", Icon = "Explore", Order = 2, Permission = HeadwayAuthorisation.ADMIN };
            var configurationCategory = new Category { Name = "Configuration", Icon = "AppSettingsAlt", Order = 3, Permission = HeadwayAuthorisation.ADMIN };
            var developerToolsCategory = new Category { Name = "Developer Tools", Icon = "Build", Order = 4, Permission = HeadwayAuthorisation.DEVELOPER };

            dbContext.Categories.Add(authorisationCatgory);
            dbContext.Categories.Add(configurationCategory);
            dbContext.Categories.Add(developerToolsCategory);

            var usersMenuItem = new MenuItem { Name = "Users", Icon = "SupervisedUserCircle", NavigatePage = "Page", Order = 1, Permission = HeadwayAuthorisation.ADMIN, Config = "Users" };
            var rolesMenuItem = new MenuItem { Name = "Roles", Icon = "LockOutlined", NavigatePage = "Page", Order = 2, Permission = HeadwayAuthorisation.ADMIN, Config = "Roles" };
            var permissionsMenuItem = new MenuItem { Name = "Permissions", Icon = "Key", NavigatePage = "Page", Order = 3, Permission = HeadwayAuthorisation.ADMIN, Config = "Permissions" };

            var modulesMenuItem = new MenuItem { Name = "Modules", Icon = "AutoAwesomeMosaic", NavigatePage = "Page", Order = 1, Permission = HeadwayAuthorisation.ADMIN, Config = "Modules" };
            var categoriesMenuItem = new MenuItem { Name = "Categories", Icon = "AutoAwesomeMotion", NavigatePage = "Page", Order = 2, Permission = HeadwayAuthorisation.ADMIN, Config = "Categories" };
            var menuItemsMenuItem = new MenuItem { Name = "MenuItems", Icon = "Article", NavigatePage = "Page", Order = 3, Permission = HeadwayAuthorisation.ADMIN, Config = "MenuItems" };

            var configureMenuItem = new MenuItem { Name = "Configure", Icon = "Settings", NavigatePage = "Page", Order = 1, Permission = HeadwayAuthorisation.ADMIN, Config = "Configs" };
            var demoMenuItem = new MenuItem { Name = "Demo", Icon = "InfoOutlined", NavigatePage = "Page", Order = 2, Permission = HeadwayAuthorisation.DEVELOPER, Config = "DemoModels" };

            dbContext.MenuItems.Add(usersMenuItem);
            dbContext.MenuItems.Add(rolesMenuItem);
            dbContext.MenuItems.Add(permissionsMenuItem);
            dbContext.MenuItems.Add(modulesMenuItem);
            dbContext.MenuItems.Add(categoriesMenuItem);
            dbContext.MenuItems.Add(menuItemsMenuItem);
            dbContext.MenuItems.Add(configureMenuItem);
            dbContext.MenuItems.Add(demoMenuItem);

            authorisationCatgory.MenuItems.Add(usersMenuItem);
            authorisationCatgory.MenuItems.Add(rolesMenuItem);
            authorisationCatgory.MenuItems.Add(permissionsMenuItem);
            navigationCatgory.MenuItems.Add(modulesMenuItem);
            navigationCatgory.MenuItems.Add(categoriesMenuItem);
            navigationCatgory.MenuItems.Add(menuItemsMenuItem);
            configurationCategory.MenuItems.Add(configureMenuItem);
            developerToolsCategory.MenuItems.Add(demoMenuItem);

            administration.Categories.Add(authorisationCatgory);
            administration.Categories.Add(navigationCatgory);
            administration.Categories.Add(configurationCategory);
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
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
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
                Document = "Headway.Razor.Controls.Documents.Document`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Permissions"
            };

            dbContext.Configs.Add(permissionConfig);

            var permissionConfigContainer = new ConfigContainer { Name = "Permission Div", Code = "PERMISSION_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            permissionConfig.ConfigContainers.Add(permissionConfigContainer);

            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", IsIdentity = true, Order = 1, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });

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
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
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
                Document = "Headway.Razor.Controls.Documents.Document`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Roles"
            };

            dbContext.Configs.Add(roleConfig);

            var roleConfigContainer = new ConfigContainer { Name = "Role Div", Code = "ROLE_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            roleConfig.ConfigContainers.Add(roleConfigContainer);

            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RoleId", Label = "Role Id", IsIdentity = true, Order = 1, ConfigContainer = roleConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = roleConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3, ConfigContainer = roleConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionChecklist", Label = "Permissions", Order = 4, ConfigContainer = roleConfigContainer, Component = "Headway.Razor.Controls.Components.CheckList, Headway.Razor.Controls" });

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
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
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
                Document = "Headway.Razor.Controls.Documents.Document`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Users"
            };

            dbContext.Configs.Add(userConfig);

            var userConfigContainer = new ConfigContainer { Name = "User Div", Code = "USER_DIV", Order = 1, Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls" };
            var authConfigContainer = new ConfigContainer { Name = "Auth Div", Code = "AUTH_DIV", ParentCode = "USER_DEV", Order = 1, Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", ComponentArgs = "Name=LayoutHorizontal;Value=True" };
            userConfigContainer.ConfigContainers.Add(authConfigContainer);
            userConfig.ConfigContainers.Add(userConfigContainer);
            userConfig.ConfigContainers.Add(authConfigContainer);

            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UserId", Label = "User Id", IsIdentity = true, Order = 1, ConfigContainer = userConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UserName", Label = "User Name", IsTitle = true, Order = 2, ConfigContainer = userConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Email", Label = "Email", Order = 3, ConfigContainer = userConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RoleChecklist", Label = "Roles", Order = 4, ConfigContainer = authConfigContainer, Component = "Headway.Razor.Controls.Components.CheckList, Headway.Razor.Controls" });
            userConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionChecklist", Label = "Permissions", Order = 5, ConfigContainer = authConfigContainer, Component = "Headway.Razor.Controls.Components.CheckList, Headway.Razor.Controls" });

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
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
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
                Document = "Headway.Razor.Controls.Documents.Document`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Modules"
            };

            dbContext.Configs.Add(moduleConfig);

            var moduleConfigContainer = new ConfigContainer { Name = "Module Div", Code = "MODULE_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            moduleConfig.ConfigContainers.Add(moduleConfigContainer);

            moduleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ModuleId", Label = "Module Id", IsIdentity = true, Order = 1, ConfigContainer = moduleConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            moduleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = moduleConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            moduleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 3, ConfigContainer = moduleConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });
            moduleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Permission", Label = "Permission", Order = 4, ConfigContainer = moduleConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.PERMISSIONS_OPTION_ITEMS}" });

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
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
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
                Document = "Headway.Razor.Controls.Documents.Document`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Categories"
            };

            dbContext.Configs.Add(categoryConfig);

            var categoryConfigContainer = new ConfigContainer { Name = "Category Div", Code = "CATEGORY_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            categoryConfig.ConfigContainers.Add(categoryConfigContainer);

            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CategoryId", Label = "Category Id", IsIdentity = true, Order = 1, ConfigContainer = categoryConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = categoryConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 3, ConfigContainer = categoryConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });
            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Permission", Label = "Permission", Order = 4, ConfigContainer = categoryConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.PERMISSIONS_OPTION_ITEMS}" });
            categoryConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Module", Label = "Module", Order = 5, ConfigContainer = categoryConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.MODULES_OPTION_ITEMS}|Name={Options.DISPLAY_FIELD};Value=Name|Name={Args.MODEL};Value=Headway.Core.Model.Module, Headway.Core|Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownComplex`1, Headway.Razor.Controls" });

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
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
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
                Document = "Headway.Razor.Controls.Documents.Document`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "MenuItems"
            };

            dbContext.Configs.Add(menuItemConfig);

            var menuItemConfigContainer = new ConfigContainer { Name = "Menu Item Div", Code = "MENUITEM_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            menuItemConfig.ConfigContainers.Add(menuItemConfigContainer);

            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "MenuItemId", Label = "Menu Item Id", IsIdentity = true, Order = 1, ConfigContainer = menuItemConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = menuItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ImageClass", Label = "Image Class", IsTitle = true, Order = 3, ConfigContainer = menuItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Config", Label = "Config", Order = 3, ConfigContainer = menuItemConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONFIG_OPTION_ITEMS}" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigatePage", Label = "Navigate Page", Order = 3, ConfigContainer = menuItemConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(PageOptionItems)}" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 4, ConfigContainer = menuItemConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Permission", Label = "Permission", Order = 5, ConfigContainer = menuItemConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.PERMISSIONS_OPTION_ITEMS}" });
            menuItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Category", Label = "Category", Order = 6, ConfigContainer = menuItemConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CATEGORIES_OPTION_ITEMS}|Name={Options.DISPLAY_FIELD};Value=Name|Name={Args.MODEL};Value=Headway.Core.Model.Category, Headway.Core|Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownComplex`1, Headway.Razor.Controls" });

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
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
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
                Document = "Headway.Razor.Controls.Documents.TabDocument`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Configs"
            };

            dbContext.Configs.Add(configConfig);

            var configConfigContainer1 = new ConfigContainer { Name = "Model Div", Code = "MODEL DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Model", Order = 1 };
            var configConfigContainer2 = new ConfigContainer { Name = "Fields Div", Code = "FIELDS_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Fields", Order = 2 };
            var configConfigContainer3 = new ConfigContainer { Name = "Containers Div", Code = "CONTAINERS_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Containers", Order = 3 };

            configConfig.ConfigContainers.Add(configConfigContainer1);
            configConfig.ConfigContainers.Add(configConfigContainer2);
            configConfig.ConfigContainers.Add(configConfigContainer3);

            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigId", Label = "Config Id", IsIdentity = true, Order = 1, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", IsTitle = true, Order = 3, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", IsTitle = false, Order = 4, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Model", Label = "Model", Order = 5, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelOptionItems)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ModelApi", Label = "Model Api", Order = 6, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONTROLLER_OPTION_ITEMS}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OrderModelBy", Label = "Order Model By", Order = 7, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelFieldsOptionItems)}|Name={Args.LINK_SOURCE};VALUE=Model" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Document", Label = "Document", Order = 8, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(DocumentOptionItems)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigatePage", Label = "Navigate Page", Order = 9, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(PageOptionItems)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateProperty", Label = "Navigate Property", Order = 10, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelFieldsOptionItems)}|Name={Args.LINK_SOURCE};Value=Model" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateConfig", Label = "Navigate Config", Order = 11, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONFIG_OPTION_ITEMS}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigItems", Label = "Config Items", Order = 12, ConfigContainer = configConfigContainer2, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "ConfigItem", ComponentArgs = $"Name={Args.LIST_CONFIG};Value=ConfigItemsListDetail" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainers", Label = "Config Containers", Tooltip = "Drag and drop containers into nested hierarchy", Order = 13, ConfigContainer = configConfigContainer3, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "ConfigContainer", ComponentArgs = $"Name={Args.UNIQUE_PROPERTY};Value={Args.CODE}|Name={Args.UNIQUE_PARENT_PROPERTY};Value={Args.CODE_PARENT}|Name={Args.LABEL_PROPERTY};Value=Label|Name={Args.LIST_PROPERTY};Value=ConfigContainers" });
            dbContext.SaveChanges();
        }

        private static void ConfigItemConfig()
        {
            var configItemConfig = new Config
            {
                Name = "ConfigItem",
                Title = "ConfigItem",
                Description = "Config Item maps to a custom object's property to be rendered on the screeen",
                Model = "Headway.Core.Model.ConfigItem, Headway.Core",
                ModelApi = "Configuration",
                Document = "Headway.Razor.Controls.Documents.ListDetail`1, Headway.Razor.Controls"
            };

            dbContext.Configs.Add(configItemConfig);

            var configItemConfigContainer = new ConfigContainer { Name = "Config Item Div", Code = "CONFIG_ITEM_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            configItemConfig.ConfigContainers.Add(configItemConfigContainer);

            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigItemId", Label = "Config Item Id", IsIdentity = true, Order = 1, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PropertyName", Label = "PropertyName", Order = 2, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelFieldsOptionItems)}|Name={Args.MODEL};Value={nameof(Config)}" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Label", Label = "Label", Order = 3, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Tooltip", Label = "Tooltip", Order = 4, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "IsIdentity", Label = "IsIdentity", Order = 5, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Checkbox, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "IsTitle", Label = "IsTitle", Order = 6, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Checkbox, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 7, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Component", Label = "Component", Order = 8, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ComponentOptionItems)}" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ComponentArgs", Label = "ComponentArgs", Order = 9, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigName", Label = "Config Name", Order = 10, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONFIG_OPTION_ITEMS}" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainer", Label = "Container", Order = 11, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONFIG_CONTAINERS}|Name={Args.SEARCH_PARAMETER};Value=Config|Name={Options.DISPLAY_FIELD};Value=Name|Name={Args.MODEL};Value=Headway.Core.Model.ConfigContainer, Headway.Core|Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownComplex`1, Headway.Razor.Controls" });

            dbContext.SaveChanges();
        }

        private static void ConfigItemsListDetailConfig()
        {
            var configItemsListDetailConfig = new Config
            {
                Name = "ConfigItemsListDetail",
                Title = "ConfigItemsListDetail",
                Description = "List of custom object properties to be rendered on the screeen",
                Model = "Headway.Core.Model.ConfigItem, Headway.Core",
                ModelApi = "Configuration",
                OrderModelBy = "Order"
            };

            dbContext.Configs.Add(configItemsListDetailConfig);

            configItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PropertyName", Label = "Property Name", Order = 1 });
            configItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 2 });

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
                Document = "Headway.Razor.Controls.Documents.TreeDetail`1, Headway.Razor.Controls"
            };

            dbContext.Configs.Add(configContainerConfig);

            var configContainerConfigContainer = new ConfigContainer { Name = "Config Container Div", Code = "CONFIG_CONTAINER_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            configContainerConfig.ConfigContainers.Add(configContainerConfigContainer);

            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainerId", Label = "Config Container Id", IsIdentity = true, Order = 1, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Code", Label = "Code", IsIdentity = true, Order = 2, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ParentCode", Label = "Parent Code", IsIdentity = true, Order = 3, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 4, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Label", Label = "Label", Order = 5, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ComponentArgs", Label = "Component Args", Order = 6, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 7, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Container", Label = "Container", Order = 8, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ContainerOptionItems)}" });
            dbContext.SaveChanges();
        }

        private static void DemoModelsConfig()
        {
            var demoModelsConfig = new Config
            {
                Name = "DemoModels",
                Title = "DemoModels",
                Description = "List of Demo Models to demonstrate rendering objects on a page",
                Model = "Headway.Core.Model.DemoModel, Headway.Core",
                ModelApi = "DemoModel",
                OrderModelBy = "DemoModelId",
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateProperty = "DemoModelId",
                NavigateConfig = "DemoModel",
                NavigateResetBreadcrumb = true
            };

            dbContext.Configs.Add(demoModelsConfig);

            demoModelsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelId", Label = "Demo Model Id", Order = 1 });
            demoModelsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Text", Label = "Text", Order = 2 });

            dbContext.SaveChanges();
        }

        private static void DemoModelConfig()
        {
            var demoModelConfig = new Config
            {
                Name = "DemoModel",
                Title = "Demo",
                Description = "Demonstrate rendering an object on a page",
                Model = "Headway.Core.Model.DemoModel, Headway.Core",
                ModelApi = "DemoModel",
                Document = "Headway.Razor.Controls.Documents.TabDocument`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "DemoModels"
            };

            dbContext.Configs.Add(demoModelConfig);

            var demoModelContainer1 = new ConfigContainer { Name = "Model Components Div", Code = "MODEL_COMPONENTS_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Model Components", Order = 1 };
            var demoModelContainer2 = new ConfigContainer { Name = "List Component Div", Code = "LIST_COMPONENT_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "List Component", Order = 2 };
            var demoModelContainer3 = new ConfigContainer { Name = "Tree Component Div", Code = "TREE_COMPONENT_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Tree Component", Order = 3 };

            demoModelConfig.ConfigContainers.Add(demoModelContainer1);
            demoModelConfig.ConfigContainers.Add(demoModelContainer2);
            demoModelConfig.ConfigContainers.Add(demoModelContainer3);

            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelId", Label = "Demo Model Id", IsIdentity = true, Order = 1, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls", Tooltip = "Text" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Text", Label = "Text", IsTitle = true, Order = 2, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls", Tooltip = "Text" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "TextMultiline", Label = "TextMultiline", Order = 3, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.TextMultiline, Headway.Razor.Controls", Tooltip = "TextMultiline", ComponentArgs = $"Name={Args.TEXT_MULTILINE_ROWS};Value=3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Integer", Label = "Integer", Order = 4, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls", Tooltip = "Integer" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Checkbox", Label = "Checkbox", Order = 5, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Checkbox, Headway.Razor.Controls", Tooltip = "Checkbox" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Decimal", Label = "Decimal", Order = 6, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Decimal, Headway.Razor.Controls", Tooltip = "Decimal" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OptionHorizontal", Label = "OptionHorizontal", Order = 7, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Option.OptionHorizontal, Headway.Razor.Controls", Tooltip = "OptionHorizontal", ComponentArgs = $"Name=one;Value=Option 1|Name=two;Value=Option 2|Name=three;Value=Option 3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Date", Label = "Date", Order = 8, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Date, Headway.Razor.Controls", Tooltip = "Date" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OptionVertical", Label = "OptionVertical", Order = 9, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Option.OptionVertical, Headway.Razor.Controls", Tooltip = "OptionVertical", ComponentArgs = $"Name=one;Value=Option 1|Name=two;Value=Option 2|Name=three;Value=Option 3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Dropdown", Label = "Dropdown", Order = 10, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", Tooltip = "Dropdown", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.STATIC_OPTION_ITEMS}|Name=one;Value=Option 1|Name=two;Value=Option 2|Name=three;Value=Option 3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DropdownComplex", Label = "DropdownComplex", Order = 11, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.COMPLEX_OPTION_ITEMS}|Name={Options.DISPLAY_FIELD};Value=Name|Name={Args.MODEL};Value=Headway.Core.Model.DemoModelComplexProperty, Headway.Core|Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownComplex`1, Headway.Razor.Controls" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelItems", Label = "Demo Model Items", Order = 12, ConfigContainer = demoModelContainer2, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "DemoModelItem", ComponentArgs = $"Name={Args.LIST_CONFIG};Value=DemoModelItemsListDetail" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelTreeItems", Label = "Demo Model Tree Items", Order = 13, Tooltip = "Drag and drop items in a nested tree hierarchy", ConfigContainer = demoModelContainer3, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "DemoModelTreeItem", ComponentArgs = $"Name={Args.UNIQUE_PROPERTY};Value={Args.CODE}|Name={Args.UNIQUE_PARENT_PROPERTY};Value={Args.CODE_PARENT}|Name={Args.LABEL_PROPERTY};Value=Name|Name={Args.LIST_PROPERTY};Value=DemoModelTreeItems" });

            dbContext.SaveChanges();
        }

        private static void DemoModelItemConfig()
        {
            var demoModelItemConfig = new Config
            {
                Name = "DemoModelItem",
                Title = "DemoModelItem",
                Description = "Config for rendering a custom object associated with the Demo Model",
                Model = "Headway.Core.Model.DemoModelItem, Headway.Core",
                ModelApi = "DemoModel",
                Document = "Headway.Razor.Controls.Documents.ListDetail`1, Headway.Razor.Controls"
            };

            dbContext.Configs.Add(demoModelItemConfig);

            var demoModelItemConfigContainer = new ConfigContainer { Name = "Demo Model Item Div", Code = "DEMO_MODEL_ITEM_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            demoModelItemConfig.ConfigContainers.Add(demoModelItemConfigContainer);

            demoModelItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelItemId", Label = "Item Id", IsIdentity = true, Order = 1, ConfigContainer = demoModelItemConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            demoModelItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2, ConfigContainer = demoModelItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            demoModelItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 3, ConfigContainer = demoModelItemConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });

            dbContext.SaveChanges();
        }

        private static void DemoModelItemsListDetailConfig()
        {
            var demoModelItemsListDetailConfig = new Config
            {
                Name = "DemoModelItemsListDetail",
                Title = "DemoModelItemsListDetail",
                Description = "Config for rendering a list of custom objects associated with the Demo Model",
                Model = "Headway.Core.Model.DemoModelItem, Headway.Core",
                ModelApi = "DemoModel",
                OrderModelBy = "Order"
            };

            dbContext.Configs.Add(demoModelItemsListDetailConfig);

            demoModelItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 1 });
            demoModelItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 2 });

            dbContext.SaveChanges();
        }

        private static void DemoModelTreeItemConfig()
        {
            var demoModelTreeItemConfig = new Config
            {
                Name = "DemoModelTreeItem",
                Title = "DemoModelTreeItem",
                Description = "Config for rendering a treeview of custom objects associated with the Demo Model",
                Model = "Headway.Core.Model.DemoModelTreeItem, Headway.Core",
                ModelApi = "DemoModel",
                Document = "Headway.Razor.Controls.Documents.TreeDetail`1, Headway.Razor.Controls"
            };

            dbContext.Configs.Add(demoModelTreeItemConfig);

            var demoModelTreeItemConfigContainer = new ConfigContainer { Name = "Demo Model Tree Item Div", Code = "DEMO_MODEL_TREE_ITEM_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            demoModelTreeItemConfig.ConfigContainers.Add(demoModelTreeItemConfigContainer);

            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelTreeItemId", Label = "Tree Item Id", IsIdentity = true, Order = 1, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ItemCode", Label = "Item Code", Order = 2, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 3, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 4, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });

            dbContext.SaveChanges();
        }
    }
}