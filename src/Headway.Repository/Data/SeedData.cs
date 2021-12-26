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

            if (!applicationDbContext.Users.Any()
                && !applicationDbContext.Permissions.Any()
                && !applicationDbContext.Roles.Any())
            {
                admin = new Permission { Name = "Admin", Description = "Administrator" };
                user = new Permission { Name = "User", Description = "Headway User" };
                applicationDbContext.Permissions.Add(admin);
                applicationDbContext.Permissions.Add(user);
                applicationDbContext.SaveChanges();

                var alice = new User { UserName = "alice", Email = "alice@email.com" };
                var bob = new User { UserName = "bob", Email = "bob@email.com" };
                var jane = new User { UserName = "jane", Email = "jane@email.com" };
                var will = new User { UserName = "will", Email = "will@email.com" };
                applicationDbContext.Users.Add(alice);
                applicationDbContext.Users.Add(bob);
                applicationDbContext.Users.Add(jane);
                applicationDbContext.Users.Add(will);
                applicationDbContext.SaveChanges();

                var adminRole = new Role { Name = "Admin", Description = "Administrator Role" };
                var userRole = new Role { Name = "User", Description = "Headway User Role" };
                applicationDbContext.Roles.Add(adminRole);
                applicationDbContext.Roles.Add(userRole);
                applicationDbContext.SaveChanges();

                adminRole.Permissions.Add(admin);
                userRole.Permissions.Add(user);
                applicationDbContext.SaveChanges();

                alice.Roles.Add(adminRole);
                alice.Roles.Add(userRole);
                bob.Roles.Add(adminRole);
                bob.Roles.Add(userRole);
                jane.Roles.Add(userRole);
                will.Roles.Add(userRole);
                applicationDbContext.SaveChanges();
            }

            if (!applicationDbContext.Modules.Any()
                && !applicationDbContext.Categories.Any()
                && !applicationDbContext.MenuItems.Any())
            {
                var home = new Module { Name = "Home", Order = 1, Permission = user.Name };
                var administration = new Module { Name = "Administration", Order = 2, Permission = admin.Name };
                applicationDbContext.Modules.Add(home);
                applicationDbContext.Modules.Add(administration);
                applicationDbContext.SaveChanges();

                var homeCategory = new Category { Name = "Home Category", Order = 1, Permission = user.Name };
                var authorisationCatgory = new Category { Name = "Authorisation", Order = 1, Permission = admin.Name };
                var configurationCategory = new Category { Name = "Configuration", Order = 2, Permission = admin.Name };
                applicationDbContext.Categories.Add(homeCategory);
                applicationDbContext.Categories.Add(authorisationCatgory);
                applicationDbContext.Categories.Add(configurationCategory);
                applicationDbContext.SaveChanges();

                home.Categories.Add(homeCategory);
                applicationDbContext.SaveChanges();

                administration.Categories.Add(authorisationCatgory);
                administration.Categories.Add(configurationCategory);
                applicationDbContext.SaveChanges();

                var homeMenuItem = new MenuItem { Name = "Home", ImageClass = "oi oi-home", NavigateTo = "/", Order = 1, Permission = user.Name, Config = "Home" };
                var usersMenuItem = new MenuItem { Name = "Users", ImageClass = "oi oi-person", NavigateTo = "Page", Order = 1, Permission = admin.Name, Config = "Users" };
                var rolesMenuItem = new MenuItem { Name = "Roles", ImageClass = "oi oi-lock-locked", NavigateTo = "Page", Order = 2, Permission = admin.Name, Config = "Roles" };
                var permissionsMenuItem = new MenuItem { Name = "Permissions", ImageClass = "oi oi-key", NavigateTo = "Page", Order = 3, Permission = admin.Name, Config = "Permissions" };
                var configureMenuItem = new MenuItem { Name = "Configure", ImageClass = "oi oi-cog", NavigateTo = "Page", Order = 1, Permission = admin.Name, Config = "Configs" };
                applicationDbContext.MenuItems.Add(homeMenuItem);
                applicationDbContext.MenuItems.Add(usersMenuItem);
                applicationDbContext.MenuItems.Add(rolesMenuItem);
                applicationDbContext.MenuItems.Add(permissionsMenuItem);
                applicationDbContext.MenuItems.Add(configureMenuItem);
                applicationDbContext.SaveChanges();

                homeCategory.MenuItems.Add(homeMenuItem);
                applicationDbContext.SaveChanges();

                authorisationCatgory.MenuItems.Add(usersMenuItem);
                authorisationCatgory.MenuItems.Add(rolesMenuItem);
                authorisationCatgory.MenuItems.Add(permissionsMenuItem);
                applicationDbContext.SaveChanges();

                configurationCategory.MenuItems.Add(configureMenuItem);
                applicationDbContext.SaveChanges();
            }

            if (!applicationDbContext.Configs.Any()
               && !applicationDbContext.ConfigItems.Any())
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

                var configItemsListDetailConfig = new Config
                {
                    Name = "ConfigItemsListDetail",
                    Title = "ConfigItemsListDetail",
                    Description = "List of config items for a config",
                    Model = "Headway.Core.Model.ConfigItem, Headway.Core",
                    ModelApi = "Configuration",
                    OrderModelBy = "Order"
                };

                var configItemConfig = new Config
                {
                    Name = "ConfigItem",
                    Title = "ConfigItem",
                    Description = "Create, update or delete a config item for a config",
                    Model = "Headway.Core.Model.ConfigItem, Headway.Core",
                    ModelApi = "Configuration",
                    Document = "Headway.Razor.Controls.Documents.ListDetail`1, Headway.Razor.Controls"
                };

                var configContainersListDetailConfig = new Config
                {
                    Name = "ConfigContainersListDetail",
                    Title = "ConfigContainersListDetail",
                    Description = "List of config containers for a config",
                    Model = "Headway.Core.Model.ConfigContainer, Headway.Core",
                    ModelApi = "Configuration",
                    OrderModelBy = "Order"
                };

                var configContainerConfig = new Config
                {
                    Name = "ConfigContainer",
                    Title = "ConfigContainer",
                    Description = "Create, update or delete a config container for a config",
                    Model = "Headway.Core.Model.ConfigContainer, Headway.Core",
                    ModelApi = "Configuration",
                    Document = "Headway.Razor.Controls.Documents.TreeDetail`1, Headway.Razor.Controls"
                };

                applicationDbContext.Configs.Add(configsConfig);
                applicationDbContext.Configs.Add(configConfig);
                applicationDbContext.Configs.Add(configItemsListDetailConfig);
                applicationDbContext.Configs.Add(configItemConfig);
                applicationDbContext.Configs.Add(configContainersListDetailConfig);
                applicationDbContext.Configs.Add(configContainerConfig);
                applicationDbContext.Configs.Add(permissionsConfig);
                applicationDbContext.Configs.Add(permissionConfig);
                applicationDbContext.SaveChanges();

                // Permission//////////////////
                var permissionConfigContainer = new ConfigContainer { Name = "Permission Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1, IsRootContainer = true };
                permissionConfig.ConfigContainers.Add(permissionConfigContainer);
                applicationDbContext.SaveChanges();

                permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", IsIdentity = true, Order = 1, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
                permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
                permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3, ConfigContainer = permissionConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
                applicationDbContext.SaveChanges();
                ////////////////////////////////

                // ConfigItem //////////////////
                var configItemConfigContainer = new ConfigContainer { Name = "Config Item Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Order = 1, IsRootContainer = true };
                configItemConfig.ConfigContainers.Add(configItemConfigContainer);
                applicationDbContext.SaveChanges();

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
                /////////////////////////////////////

                // ConfigContainer //////////////////
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
                ////////////////////////////////

                // Config //////////////////////
                var configConfigContainer1 = new ConfigContainer { Name = "Model Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Model", Order = 1, IsRootContainer = true };
                var configConfigContainer2 = new ConfigContainer { Name = "Fields Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Fields", Order = 2, IsRootContainer = true };
                var configConfigContainer3 = new ConfigContainer { Name = "Containers Div", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Containers", Order = 3, IsRootContainer = true };

                configConfig.ConfigContainers.Add(configConfigContainer1);
                configConfig.ConfigContainers.Add(configConfigContainer2);
                configConfig.ConfigContainers.Add(configConfigContainer3);
                applicationDbContext.SaveChanges();

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
                ////////////////////////////////

                // Permissions /////////////////
                permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", Order = 1 });
                permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
                permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3 });
                applicationDbContext.SaveChanges();
                /////////////////////////////////

                // Configs //////////////////////
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigId", Label = "Config Id", Order = 1 });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3 });
                applicationDbContext.SaveChanges();
                ////////////////////////////////

                // Config Items /////////////////
                configItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PropertyName", Label = "Property Name", Order = 1 });
                configItemsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 2 });
                applicationDbContext.SaveChanges();
                ////////////////////////////////
 
                // Config Containers////////////
                configContainersListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 1 });
                configContainersListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Label", Label = "Label", Order = 2 });
                applicationDbContext.SaveChanges();
                ////////////////////////////////
            }
        }
    }
}
