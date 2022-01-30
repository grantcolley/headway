using Headway.Core.Constants;
using Headway.Core.Model;
using Headway.Core.Options;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Headway.Repository.Data
{
    public class SeedData
    {
        private static Permission user = null;
        private static Permission admin = null;
        private static Permission developer = null;

        private static Role developerRole = null;
        private static Role adminRole = null;
        private static Role userRole = null;

        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            TruncateTables(applicationDbContext);

            Permissions(applicationDbContext);
            Roles(applicationDbContext);
            Users(applicationDbContext);

            DemoModels(applicationDbContext);

            AdministrationNavigationConfig(applicationDbContext);

            PermissionsConfig(applicationDbContext);
            PermissionConfig(applicationDbContext);

            RolesConfig(applicationDbContext);
            RoleConfig(applicationDbContext);

            UsersConfig(applicationDbContext);
            UserConfig(applicationDbContext);

            ConfigsConfig(applicationDbContext);
            ConfigConfig(applicationDbContext);
            ConfigItemConfig(applicationDbContext);
            ConfigItemsListDetailConfig(applicationDbContext);
            ConfigContainerConfig(applicationDbContext);

            DemoModelsConfig(applicationDbContext);
            DemoModelConfig(applicationDbContext);
            DemoModelItemConfig(applicationDbContext);
            DemoModelItemsListDetailConfig(applicationDbContext);
            DemoModelTreeItemConfig(applicationDbContext);
        }

        private static void TruncateTables(ApplicationDbContext applicationDbContext)
        {
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE RoleUser");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionUser");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE PermissionRole");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DELETE FROM Users");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Users, RESEED, 1)");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DELETE FROM Roles");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Roles, RESEED, 1)");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DELETE FROM Permissions");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Permissions, RESEED, 1)");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE DemoModelItems");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE DemoModelTreeItems");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DELETE FROM DemoModels");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (DemoModels, RESEED, 1)");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE MenuItems");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DELETE FROM Categories");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Categories, RESEED, 1)");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DELETE FROM Modules");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Modules, RESEED, 1)");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE ConfigItems");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DELETE FROM ConfigContainers");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (ConfigContainers, RESEED, 1)");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DELETE FROM Configs");
            ((DbContext)applicationDbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (Configs, RESEED, 1)");
        }

        private static void Permissions(ApplicationDbContext applicationDbContext)
        {
            user = new Permission { Name = "User", Description = "Headway User" };
            admin = new Permission { Name = "Admin", Description = "Administrator" };
            developer = new Permission { Name = "Developer", Description = "Developer" };

            applicationDbContext.Permissions.Add(user);
            applicationDbContext.Permissions.Add(admin);
            applicationDbContext.Permissions.Add(developer);

            applicationDbContext.SaveChanges();
        }

        private static void Roles(ApplicationDbContext applicationDbContext)
        {
            developerRole = new Role { Name = "Developer", Description = "Developer Role" };
            adminRole = new Role { Name = "Admin", Description = "Administrator Role" };
            userRole = new Role { Name = "User", Description = "Headway User Role" };

            applicationDbContext.Roles.Add(adminRole);
            applicationDbContext.Roles.Add(developerRole);
            applicationDbContext.Roles.Add(userRole);

            adminRole.Permissions.Add(admin);
            adminRole.Permissions.Add(user);
            developerRole.Permissions.Add(developer);
            developerRole.Permissions.Add(admin);
            developerRole.Permissions.Add(user);
            userRole.Permissions.Add(user);

            applicationDbContext.SaveChanges();
        }

        private static void Users(ApplicationDbContext applicationDbContext)
        {
            var grant = new User { UserName = "grant", Email = "grant@email.com" };
            var alice = new User { UserName = "alice", Email = "alice@email.com" };
            var bob = new User { UserName = "bob", Email = "bob@email.com" };
            var jane = new User { UserName = "jane", Email = "jane@email.com" };
            var will = new User { UserName = "will", Email = "will@email.com" };

            applicationDbContext.Users.Add(grant);
            applicationDbContext.Users.Add(alice);
            applicationDbContext.Users.Add(bob);
            applicationDbContext.Users.Add(jane);
            applicationDbContext.Users.Add(will);

            grant.Roles.Add(developerRole);
            alice.Roles.Add(adminRole);
            alice.Roles.Add(userRole);
            bob.Roles.Add(adminRole);
            bob.Roles.Add(userRole);
            jane.Roles.Add(userRole);
            will.Roles.Add(userRole);

            applicationDbContext.SaveChanges();
        }

        private static void DemoModels(ApplicationDbContext applicationDbContext)
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

            applicationDbContext.DemoModels.Add(demoModel);

            applicationDbContext.SaveChanges();
        }

        private static void AdministrationNavigationConfig(ApplicationDbContext applicationDbContext)
        {
            var administration = new Module { Name = "Administration", Order = 1, Permission = admin.Name };

            applicationDbContext.Modules.Add(administration);

            var authorisationCatgory = new Category { Name = "Authorisation", Order = 1, Permission = admin.Name };
            var configurationCategory = new Category { Name = "Configuration", Order = 2, Permission = admin.Name };
            var developerToolsCategory = new Category { Name = "Developer Tools", Order = 3, Permission = developer.Name };

            applicationDbContext.Categories.Add(authorisationCatgory);
            applicationDbContext.Categories.Add(configurationCategory);
            applicationDbContext.Categories.Add(developerToolsCategory);

            var usersMenuItem = new MenuItem { Name = "Users", ImageClass = "oi oi-person", NavigateTo = "Page", Order = 1, Permission = admin.Name, Config = "Users" };
            var rolesMenuItem = new MenuItem { Name = "Roles", ImageClass = "oi oi-lock-locked", NavigateTo = "Page", Order = 2, Permission = admin.Name, Config = "Roles" };
            var permissionsMenuItem = new MenuItem { Name = "Permissions", ImageClass = "oi oi-key", NavigateTo = "Page", Order = 3, Permission = admin.Name, Config = "Permissions" };
            var configureMenuItem = new MenuItem { Name = "Configure", ImageClass = "oi oi-cog", NavigateTo = "Page", Order = 1, Permission = admin.Name, Config = "Configs" };
            var demoMenuItem = new MenuItem { Name = "Demo", ImageClass = "oi oi-info", NavigateTo = "Page", Order = 2, Permission = developer.Name, Config = "DemoModels" };

            applicationDbContext.MenuItems.Add(usersMenuItem);
            applicationDbContext.MenuItems.Add(rolesMenuItem);
            applicationDbContext.MenuItems.Add(permissionsMenuItem);
            applicationDbContext.MenuItems.Add(configureMenuItem);
            applicationDbContext.MenuItems.Add(demoMenuItem);

            authorisationCatgory.MenuItems.Add(usersMenuItem);
            authorisationCatgory.MenuItems.Add(rolesMenuItem);
            authorisationCatgory.MenuItems.Add(permissionsMenuItem);

            configurationCategory.MenuItems.Add(configureMenuItem);

            developerToolsCategory.MenuItems.Add(demoMenuItem);

            administration.Categories.Add(authorisationCatgory);
            administration.Categories.Add(configurationCategory);
            administration.Categories.Add(developerToolsCategory);

            applicationDbContext.SaveChanges();
        }

        private static void PermissionsConfig(ApplicationDbContext applicationDbContext)
        {
            var permissionsConfig = new Config
            {
                Name = "Permissions",
                Title = "Permissions",
                Description = "Permissions list",
                Model = "Headway.Core.Model.Permission, Headway.Core",
                ModelApi = "Permissions",
                OrderModelBy = "Name",
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
                NavigateTo = "Page",
                NavigateToProperty = "PermissionId",
                NavigateToConfig = "Permission"
            };

            applicationDbContext.Configs.Add(permissionsConfig);

            permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", Order = 1 });
            permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3 });

            applicationDbContext.SaveChanges();
        }

        private static void PermissionConfig(ApplicationDbContext applicationDbContext)
        {
            var permissionConfig = new Config
            {
                Name = "Permission",
                Title = "Permission",
                Description = "Create, update or delete a permission",
                Model = "Headway.Core.Model.Permission, Headway.Core",
                ModelApi = "Permissions",
                Document = "Headway.Razor.Controls.Documents.Card`1, Headway.Razor.Controls",
                NavigateTo = "Page",
                NavigateToConfig = "Permissions"
            };

            applicationDbContext.Configs.Add(permissionConfig);

            var permissionConfigContainer = new ConfigContainer { Name = "Permission Div", Code = "PERMISSION_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            permissionConfig.ConfigContainers.Add(permissionConfigContainer);

            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", IsIdentity = true, Order = 1, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });

            applicationDbContext.SaveChanges();
        }

        private static void RolesConfig(ApplicationDbContext applicationDbContext)
        {
            var rolesConfig = new Config
            {
                Name = "Roles",
                Title = "Roles",
                Description = "Roles list",
                Model = "Headway.Core.Model.Role, Headway.Core",
                ModelApi = "Roles",
                OrderModelBy = "Name",
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
                NavigateTo = "Page",
                NavigateToProperty = "RoleId",
                NavigateToConfig = "Role"
            };

            applicationDbContext.Configs.Add(rolesConfig);

            rolesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RoleId", Label = "Role Id", Order = 1 });
            rolesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            rolesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3 });

            applicationDbContext.SaveChanges();
        }

        private static void RoleConfig(ApplicationDbContext applicationDbContext)
        {
            var roleConfig = new Config
            {
                Name = "Role",
                Title = "Role",
                Description = "Create, update or delete a role",
                Model = "Headway.Core.Model.Role, Headway.Core",
                ModelApi = "Roles",
                Document = "Headway.Razor.Controls.Documents.Card`1, Headway.Razor.Controls",
                NavigateTo = "Page",
                NavigateToConfig = "Roles"
            };

            applicationDbContext.Configs.Add(roleConfig);

            var roleConfigContainer = new ConfigContainer { Name = "Role Div", Code = "ROLE_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            roleConfig.ConfigContainers.Add(roleConfigContainer);

            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RoleId", Label = "Role Id", IsIdentity = true, Order = 1, ConfigContainer = roleConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = roleConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3, ConfigContainer = roleConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            roleConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionChecklist", Label = "Permissions", Order = 4, ConfigContainer = roleConfigContainer, Component = "Headway.Razor.Controls.Components.CheckList, Headway.Razor.Controls" });

            applicationDbContext.SaveChanges();
        }

        private static void UsersConfig(ApplicationDbContext applicationDbContext)
        {
            var usersConfig = new Config
            {
                Name = "Users",
                Title = "Users",
                Description = "Users list",
                Model = "Headway.Core.Model.User, Headway.Core",
                ModelApi = "Users",
                OrderModelBy = "UserName",
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
                NavigateTo = "Page",
                NavigateToProperty = "UserId",
                NavigateToConfig = "User"
            };

            applicationDbContext.Configs.Add(usersConfig);

            usersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UserId", Label = "User Id", Order = 1 });
            usersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "UserName", Label = "User Name", Order = 2 });
            usersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Email", Label = "Email", Order = 3 });

            applicationDbContext.SaveChanges();
        }

        private static void UserConfig(ApplicationDbContext applicationDbContext)
        {
            var userConfig = new Config
            {
                Name = "User",
                Title = "User",
                Description = "Create, update or delete a user",
                Model = "Headway.Core.Model.User, Headway.Core",
                ModelApi = "Users",
                Document = "Headway.Razor.Controls.Documents.Card`1, Headway.Razor.Controls",
                NavigateTo = "Page",
                NavigateToConfig = "Users"
            };

            applicationDbContext.Configs.Add(userConfig);

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

            applicationDbContext.SaveChanges();
        }

        private static void ConfigsConfig(ApplicationDbContext applicationDbContext)
        {
            var configsConfig = new Config
            {
                Name = "Configs",
                Title = "Configs",
                Description = "Configs list",
                Model = "Headway.Core.Model.Config, Headway.Core",
                ModelApi = "Configuration",
                OrderModelBy = "Name",
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
                NavigateTo = "Page",
                NavigateToProperty = "ConfigId",
                NavigateToConfig = "Config"
            };

            applicationDbContext.Configs.Add(configsConfig);

            configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigId", Label = "Config Id", Order = 1 });
            configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3 });

            applicationDbContext.SaveChanges();
        }

        private static void ConfigConfig(ApplicationDbContext applicationDbContext)
        {
            var configConfig = new Config
            {
                Name = "Config",
                Title = "Config",
                Description = "Create, update or delete a config",
                Model = "Headway.Core.Model.Config, Headway.Core",
                ModelApi = "Configuration",
                Document = "Headway.Razor.Controls.Documents.TabState`1, Headway.Razor.Controls",
                NavigateTo = "Page",
                NavigateToConfig = "Configs"
            };

            applicationDbContext.Configs.Add(configConfig);

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
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OrderModelBy", Label = "Order Model By", Order = 7, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelFieldsOptionItems)}|Name={Args.MODEL};Value={nameof(Config)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Document", Label = "Document", Order = 8, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(DocumentOptionItems)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateTo", Label = "Navigate To", Order = 9, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(PageOptionItems)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateToProperty", Label = "Navigate To Property", Order = 10, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={nameof(ModelFieldsOptionItems)}|Name={Args.MODEL};Value={nameof(Config)}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateToConfig", Label = "Navigate To Config", Order = 11, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.CONFIG_OPTION_ITEMS}" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigItems", Label = "Config Items", Order = 12, ConfigContainer = configConfigContainer2, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "ConfigItem", ComponentArgs = $"Name={Args.LIST_CONFIG};Value=ConfigItemsListDetail" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainers", Label = "Config Containers", Tooltip = "Drag and drop containers into nested hierarchy", Order = 13, ConfigContainer = configConfigContainer3, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "ConfigContainer", ComponentArgs = $"Name={Args.UNIQUE_PROPERTY};Value={Args.CODE}|Name={Args.PARENT_UNIQUE_PROPERTY};Value={Args.PARENT_CODE}|Name={Args.LABEL_PROPERTY};Value=Label|Name={Args.LIST_PROPERTY};Value=ConfigContainers" });
            applicationDbContext.SaveChanges();
        }

        private static void ConfigItemConfig(ApplicationDbContext applicationDbContext)
        {
            var configItemConfig = new Config
            {
                Name = "ConfigItem",
                Title = "ConfigItem",
                Description = "Create, update or delete a config item for a config",
                Model = "Headway.Core.Model.ConfigItem, Headway.Core",
                ModelApi = "Configuration",
                Document = "Headway.Razor.Controls.Documents.ListDetail`1, Headway.Razor.Controls"
            };

            applicationDbContext.Configs.Add(configItemConfig);

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

            applicationDbContext.SaveChanges();
        }

        private static void ConfigItemsListDetailConfig(ApplicationDbContext applicationDbContext)
        {
            var configItemsListDetailConfig = new Config
            {
                Name = "ConfigItemsListDetail",
                Title = "ConfigItemsListDetail",
                Description = "List of config items for a config",
                Model = "Headway.Core.Model.ConfigItem, Headway.Core",
                ModelApi = "Configuration",
                OrderModelBy = "Order"
            };

            applicationDbContext.Configs.Add(configItemsListDetailConfig);

            configItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PropertyName", Label = "Property Name", Order = 1 });
            configItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 2 });

            applicationDbContext.SaveChanges();
        }

        private static void ConfigContainerConfig(ApplicationDbContext applicationDbContext)
        {
            var configContainerConfig = new Config
            {
                Name = "ConfigContainer",
                Title = "ConfigContainer",
                Description = "Create, update or delete a config container for a config",
                Model = "Headway.Core.Model.ConfigContainer, Headway.Core",
                ModelApi = "Configuration",
                Document = "Headway.Razor.Controls.Documents.TreeDetail`1, Headway.Razor.Controls"
            };

            applicationDbContext.Configs.Add(configContainerConfig);

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
            applicationDbContext.SaveChanges();
        }

        private static void DemoModelsConfig(ApplicationDbContext applicationDbContext)
        {
            var demoModelsConfig = new Config
            {
                Name = "DemoModels",
                Title = "DemoModels",
                Description = "Demo Models list",
                Model = "Headway.Core.Model.DemoModel, Headway.Core",
                ModelApi = "DemoModel",
                OrderModelBy = "DemoModelId",
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
                NavigateTo = "Page",
                NavigateToProperty = "DemoModelId",
                NavigateToConfig = "DemoModel"
            };

            applicationDbContext.Configs.Add(demoModelsConfig);

            demoModelsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelId", Label = "Demo Model Id", Order = 1 });
            demoModelsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Text", Label = "Text", Order = 2 });

            applicationDbContext.SaveChanges();
        }

        private static void DemoModelConfig(ApplicationDbContext applicationDbContext)
        {
            var demoModelConfig = new Config
            {
                Name = "DemoModel",
                Title = "Demo",
                Description = "Demonstrate rendering a model with components",
                Model = "Headway.Core.Model.DemoModel, Headway.Core",
                ModelApi = "DemoModel",
                Document = "Headway.Razor.Controls.Documents.TabState`1, Headway.Razor.Controls",
                NavigateTo = "Page",
                NavigateToConfig = "DemoModels"
            };

            applicationDbContext.Configs.Add(demoModelConfig);

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
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OptionVertical", Label = "OptionVertical", Order = 9, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Option.OptionVertical, Headway.Razor.Controls", Tooltip = "OptionVertical", ComponentArgs = "$Name=one;Value=Option 1|Name=two;Value=Option 2|Name=three;Value=Option 3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelItems", Label = "Demo Model Items", Order = 10, ConfigContainer = demoModelContainer2, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "DemoModelItem", ComponentArgs = $"Name={Args.LIST_CONFIG};Value=DemoModelItemsListDetail" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelTreeItems", Label = "Demo Model Tree Items", Order = 11, Tooltip = "Drag and drop items in a nested tree hierarchy", ConfigContainer = demoModelContainer3, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "DemoModelTreeItem", ComponentArgs = $"Name={Args.UNIQUE_PROPERTY};Value={Args.CODE}|Name={Args.PARENT_UNIQUE_PROPERTY};Value={Args.PARENT_CODE}|Name={Args.LABEL_PROPERTY};Value=Name|Name={Args.LIST_PROPERTY};Value=DemoModelTreeItems" });

            applicationDbContext.SaveChanges();
        }

        private static void DemoModelItemConfig(ApplicationDbContext applicationDbContext)
        {
            var demoModelItemConfig = new Config
            {
                Name = "DemoModelItem",
                Title = "DemoModelItem",
                Description = "Create, update or delete a demo model item",
                Model = "Headway.Core.Model.DemoModelItem, Headway.Core",
                ModelApi = "DemoModel",
                Document = "Headway.Razor.Controls.Documents.ListDetail`1, Headway.Razor.Controls"
            };

            applicationDbContext.Configs.Add(demoModelItemConfig);

            var demoModelItemConfigContainer = new ConfigContainer { Name = "Demo Model Item Div", Code = "DEMO_MODEL_ITEM_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            demoModelItemConfig.ConfigContainers.Add(demoModelItemConfigContainer);

            demoModelItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelItemId", Label = "Item Id", IsIdentity = true, Order = 1, ConfigContainer = demoModelItemConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            demoModelItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2, ConfigContainer = demoModelItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            demoModelItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 3, ConfigContainer = demoModelItemConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });

            applicationDbContext.SaveChanges();
        }

        private static void DemoModelItemsListDetailConfig(ApplicationDbContext applicationDbContext)
        {
            var demoModelItemsListDetailConfig = new Config
            {
                Name = "DemoModelItemsListDetail",
                Title = "DemoModelItemsListDetail",
                Description = "List of demo model items",
                Model = "Headway.Core.Model.DemoModelItem, Headway.Core",
                ModelApi = "DemoModel",
                OrderModelBy = "Order"
            };

            applicationDbContext.Configs.Add(demoModelItemsListDetailConfig);

            demoModelItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 1 });
            demoModelItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 2 });

            applicationDbContext.SaveChanges();
        }

        private static void DemoModelTreeItemConfig(ApplicationDbContext applicationDbContext)
        {
            var demoModelTreeItemConfig = new Config
            {
                Name = "DemoModelTreeItem",
                Title = "DemoModelTreeItem",
                Description = "Create, update or delete a demo model tree item",
                Model = "Headway.Core.Model.DemoModelTreeItem, Headway.Core",
                ModelApi = "DemoModel",
                Document = "Headway.Razor.Controls.Documents.TreeDetail`1, Headway.Razor.Controls"
            };

            applicationDbContext.Configs.Add(demoModelTreeItemConfig);

            var demoModelTreeItemConfigContainer = new ConfigContainer { Name = "Demo Model Tree Item Div", Code = "DEMO_MODEL_TREE_ITEM_DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1 };

            demoModelTreeItemConfig.ConfigContainers.Add(demoModelTreeItemConfigContainer);

            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelTreeItemId", Label = "Tree Item Id", IsIdentity = true, Order = 1, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ItemCode", Label = "Item Code", Order = 2, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 3, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 4, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });

            applicationDbContext.SaveChanges();
        }
    }
}