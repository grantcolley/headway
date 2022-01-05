using Headway.Core.Model;
using System.Linq;

namespace Headway.Repository.Data
{
    public class SeedData
    {
        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            Permission user = null;
            Permission admin = null;
            Permission developer = null;

            Role developerRole = null;
            Role adminRole = null;
            Role userRole = null;

            if (!applicationDbContext.Users.Any()
                && !applicationDbContext.Permissions.Any()
                && !applicationDbContext.Roles.Any())
            {
                user = new Permission { Name = "User", Description = "Headway User" };
                admin = new Permission { Name = "Admin", Description = "Administrator" };
                developer = new Permission { Name = "Developer", Description = "Developer" };
                applicationDbContext.Permissions.Add(user);
                applicationDbContext.Permissions.Add(admin);
                applicationDbContext.Permissions.Add(developer);

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

            if (!applicationDbContext.Users.Any())
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

            if (!applicationDbContext.DemoModels.Any())
            {
                var demoModel = new DemoModel
                {
                    Description = "Demo model rendering components",
                    Text = "Sample text..."
                };
                applicationDbContext.DemoModels.Add(demoModel);
                applicationDbContext.SaveChanges();
            }

            if (!applicationDbContext.Modules.Any()
                && !applicationDbContext.Categories.Any()
                && !applicationDbContext.MenuItems.Any())
            {
                AdministrationNavigationConfig(applicationDbContext, admin, developer);
            }

            if (!applicationDbContext.Configs.Any()
               && !applicationDbContext.ConfigItems.Any())
            {
                PermissionsConfig(applicationDbContext);
                PermissionConfig(applicationDbContext);

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
        }

        private static void AdministrationNavigationConfig(ApplicationDbContext applicationDbContext, Permission admin, Permission developer)
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

            var permissionConfigContainer = new ConfigContainer { Name = "Permission Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1, IsRootContainer = true };
            permissionConfig.ConfigContainers.Add(permissionConfigContainer);

            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", IsIdentity = true, Order = 1, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });

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

            var configConfigContainer1 = new ConfigContainer { Name = "Model Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Model", Order = 1, IsRootContainer = true };
            var configConfigContainer2 = new ConfigContainer { Name = "Fields Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Fields", Order = 2, IsRootContainer = true };
            var configConfigContainer3 = new ConfigContainer { Name = "Containers Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Containers", Order = 3, IsRootContainer = true };

            configConfig.ConfigContainers.Add(configConfigContainer1);
            configConfig.ConfigContainers.Add(configConfigContainer2);
            configConfig.ConfigContainers.Add(configConfigContainer3);

            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigId", Label = "Config Id", IsIdentity = true, Order = 1, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", IsTitle = true, Order = 3, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", IsTitle = false, Order = 4, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Model", Label = "Model", Order = 5, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=ModelOptionItems" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ModelApi", Label = "Model Api", Order = 6, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=ControllerOptionItems" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OrderModelBy", Label = "Order Model By", Order = 7, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=ModelFieldsOptionItems|Name=Model;Value=Config" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Document", Label = "Document", Order = 8, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=DocumentOptionItems" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateTo", Label = "Navigate To", Order = 9, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=PageOptionItems" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateToProperty", Label = "Navigate To Property", Order = 10, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=ModelFieldsOptionItems|Name=Model;Value=Config" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateToConfig", Label = "Navigate To Config", Order = 11, ConfigContainer = configConfigContainer1, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=ConfigOptionItems" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigItems", Label = "Config Items", Order = 12, ConfigContainer = configConfigContainer2, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "ConfigItem", ComponentArgs = "Name=ListConfig;Value=ConfigItemsListDetail" });
            configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainers", Label = "Config Containers", Tooltip = "Drag and drop containers into nested hierarchy", Order = 13, ConfigContainer = configConfigContainer3, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "ConfigContainer", ComponentArgs = "Name=ModelUniqueProperty;Value=Name|Name=ModelLabelProperty;Value=Label|Name=ModelListProperty;Value=ConfigContainers" });

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

            var configItemConfigContainer = new ConfigContainer { Name = "Config Item Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1, IsRootContainer = true };
            configItemConfig.ConfigContainers.Add(configItemConfigContainer);

            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigItemId", Label = "Config Item Id", IsIdentity = true, Order = 1, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PropertyName", Label = "PropertyName", Order = 2, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=ModelFieldsOptionItems|Name=Model;Value=Config" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Label", Label = "Label", Order = 3, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Tooltip", Label = "Tooltip", Order = 4, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "IsIdentity", Label = "IsIdentity", Order = 5, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Checkbox, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "IsTitle", Label = "IsTitle", Order = 6, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Checkbox, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 7, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Component", Label = "Component", Order = 8, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=ComponentOptionItems" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ComponentArgs", Label = "ComponentArgs", Order = 9, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigName", Label = "Config Name", Order = 10, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=ConfigOptionItems" });
            configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainer", Label = "Container", Order = 11, ConfigContainer = configItemConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=ConfigContainers|Name=SearchParameter;Value=Config|Name=DisplayField;Value=Name|Name=Model;Value=Headway.Core.Model.ConfigContainer, Headway.Core|Name=Component;Value=Headway.Razor.Controls.Components.DropdownComplex`1, Headway.Razor.Controls" });

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

            var configContainerConfigContainer = new ConfigContainer { Name = "Config Container Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1, IsRootContainer = true };
            configContainerConfig.ConfigContainers.Add(configContainerConfigContainer);
            applicationDbContext.SaveChanges();

            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainerId", Label = "Config Container Id", IsIdentity = true, Order = 1, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Label", Label = "Label", Order = 3, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 4, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "IsRootContainer", Label = "IsRootContainer", Order = 5, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Checkbox, Headway.Razor.Controls" });
            configContainerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Container", Label = "Container", Order = 6, ConfigContainer = configContainerConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode;Value=ContainerOptionItems" });
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
            demoModelsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 2 });

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

            var demoModelContainer1 = new ConfigContainer { Name = "Model Components Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Model Components", Order = 1, IsRootContainer = true };
            var demoModelContainer2 = new ConfigContainer { Name = "List Component Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "List Component", Order = 2, IsRootContainer = true };
            var demoModelContainer3 = new ConfigContainer { Name = "Tree Component Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Tree Component", Order = 3, IsRootContainer = true };

            demoModelConfig.ConfigContainers.Add(demoModelContainer1);
            demoModelConfig.ConfigContainers.Add(demoModelContainer2);
            demoModelConfig.ConfigContainers.Add(demoModelContainer3);

            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Text", Label = "Text", IsTitle = true, Order = 1, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls", Tooltip = "Text" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "TextMultiline", Label = "TextMultiline", Order = 2, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.TextMultiline, Headway.Razor.Controls", Tooltip = "TextMultiline", ComponentArgs = "Name=Rows;Value=3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Integer", Label = "Integer", Order = 3, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls", Tooltip = "Integer" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Checkbox", Label = "Checkbox", Order = 4, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Checkbox, Headway.Razor.Controls", Tooltip = "Checkbox" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Decimal", Label = "Decimal", Order = 5, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Decimal, Headway.Razor.Controls", Tooltip = "Decimal" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OptionHorizontal", Label = "OptionHorizontal", Order = 6, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Option.OptionHorizontal, Headway.Razor.Controls", Tooltip = "OptionHorizontal", ComponentArgs = "Name=one;Value=Option 1|Name=two;Value=Option 2|Name=three;Value=Option 3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Date", Label = "Date", Order = 7, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Date, Headway.Razor.Controls", Tooltip = "Date" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OptionVertical", Label = "OptionVertical", Order = 8, ConfigContainer = demoModelContainer1, Component = "Headway.Razor.Controls.Components.Option.OptionVertical, Headway.Razor.Controls", Tooltip = "OptionVertical", ComponentArgs = "Name=one;Value=Option 1|Name=two;Value=Option 2|Name=three;Value=Option 3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelItems", Label = "Demo Model Items", Order = 9, ConfigContainer = demoModelContainer2, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "DemoModelItem", ComponentArgs = "Name=ListConfig;Value=DemoModelItemsListDetail" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelTreeItems", Label = "Demo Model Tree Items", Tooltip = "Drag and drop items in a nested tree hierarchy", Order = 10, ConfigContainer = demoModelContainer3, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "DemoModelTreeItem", ComponentArgs = "Name=ModelUniqueProperty;Value=DemoModelTreeItemId|Name=ModelLabelProperty;Value=Name|Name=ModelListProperty;Value=DemoModelTreeItems" });

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

            var demoModelItemConfigContainer = new ConfigContainer { Name = "Demo Model Item Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1, IsRootContainer = true };
            demoModelItemConfig.ConfigContainers.Add(demoModelItemConfigContainer);

            demoModelItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelItemId", Label = "Demo Model Item Id", IsIdentity = true, Order = 1, ConfigContainer = demoModelItemConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
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

            var demoModelTreeItemConfigContainer = new ConfigContainer { Name = "Demo Model Tree Item Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1, IsRootContainer = true };
            demoModelTreeItemConfig.ConfigContainers.Add(demoModelTreeItemConfigContainer);

            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelTreeItemId", Label = "Demo Model Tree Item Id", IsIdentity = true, Order = 1, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 3, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls" });

            applicationDbContext.SaveChanges();
        }
    }
}