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
                var usersMenuItem = new MenuItem { Name = "Users", ImageClass = "oi oi-person", NavigateTo = "list", Order = 1, Permission = admin.Name, Config = "Users" };
                var rolesMenuItem = new MenuItem { Name = "Roles", ImageClass = "oi oi-lock-locked", NavigateTo = "list", Order = 2, Permission = admin.Name, Config = "Roles" };
                var permissionsMenuItem = new MenuItem { Name = "Permissions", ImageClass = "oi oi-key", NavigateTo = "list", Order = 3, Permission = admin.Name, Config = "Permissions" };
                var configureMenuItem = new MenuItem { Name = "Configure", ImageClass = "oi oi-cog", NavigateTo = "list", Order = 1, Permission = admin.Name, Config = "Configs" };
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
                    Model = "Headway.Core.Model.Permission, Headway.Core",
                    ModelApi = "Permissions",
                    Container = "Headway.Razor.Controls.Containers.Table`1, Headway.Razor.Controls",
                    NavigateTo = "Model",
                    NavigateToProperty = "PermissionId",
                    NavigateToConfig = "Permission",
                    NavigateBack = "List",
                    NavigateBackProperty = null,
                    NavigateBackConfig = "Permissions"
                };

                var permissionConfig = new Config
                {
                    Name = "Permission",
                    Title = "Permission",
                    Model = "Headway.Core.Model.Permission, Headway.Core",
                    ModelApi = "Permissions",
                    Container = "Headway.Razor.Controls.Containers.Card`1, Headway.Razor.Controls",
                    NavigateTo = "List",
                    NavigateToProperty = null,
                    NavigateToConfig = "Permissions",
                    NavigateBack = "List",
                    NavigateBackProperty = null,
                    NavigateBackConfig = "Permissions"
                };

                var configsConfig = new Config
                {
                    Name = "Configs",
                    Title = "Configs",
                    Model = "Headway.Core.Model.Config, Headway.Core",
                    ModelApi = "Configuration",
                    Container = "Headway.Razor.Controls.Containers.Table`1, Headway.Razor.Controls",
                    NavigateTo = "Model",
                    NavigateToProperty = "ConfigId",
                    NavigateToConfig = "Config",
                    NavigateBack = "List",
                    NavigateBackProperty = null,
                    NavigateBackConfig = "Configs"
                };

                var configConfig = new Config
                {
                    Name = "Config",
                    Title = "Config",
                    Model = "Headway.Core.Model.Config, Headway.Core",
                    ModelApi = "Configuration",
                    Container = "Headway.Razor.Controls.Containers.Card`1, Headway.Razor.Controls",
                    NavigateTo = "List",
                    NavigateToProperty = null,
                    NavigateToConfig = "Configs",
                    NavigateBack = "List",
                    NavigateBackProperty = null,
                    NavigateBackConfig = "Configs"
                };

                var configItemConfig = new Config
                {
                    Name = "ConfigItem",
                    Title = "ConfigItem",
                    Model = "Headway.Core.Model.ConfigItem, Headway.Core",
                    ModelApi = "Configuration",
                    Container = "Headway.Razor.Controls.Containers.ListDetail`1, Headway.Razor.Controls",
                    NavigateTo = null,
                    NavigateToProperty = null,
                    NavigateToConfig = null,
                    NavigateBack = null,
                    NavigateBackProperty = null,
                    NavigateBackConfig = null
                };

                applicationDbContext.Configs.Add(configsConfig);
                applicationDbContext.Configs.Add(configConfig);
                applicationDbContext.Configs.Add(configItemConfig);
                applicationDbContext.Configs.Add(permissionsConfig);
                applicationDbContext.Configs.Add(permissionConfig);
                applicationDbContext.SaveChanges();

                var configConfigContainer = new ConfigContainer { Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Row = 1, Column =1, IsRootContainer = true };

                configConfig.Containers.Add(configConfigContainer);
                applicationDbContext.SaveChanges();

                permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", IsIdentity = null, IsTitle = null, Order = 1, Component = null });
                permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsIdentity = null, IsTitle = null, Order = 2, Component = null });
                permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", IsIdentity = null, IsTitle = null, Order = 3, Component = null });
                applicationDbContext.SaveChanges();

                permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", IsIdentity = true, IsTitle = false, Order = 1, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
                permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsIdentity = false, IsTitle = true, Order = 2, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
                permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", IsIdentity = false, IsTitle = false, Order = 3, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
                applicationDbContext.SaveChanges();

                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigId", Label = "Config Id", IsIdentity = null, IsTitle = null, Order = 1, Component = null });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsIdentity = null, IsTitle = null, Order = 2, Component = null });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", IsIdentity = null, IsTitle = null, Order = 3, Component = null });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Model", Label = "Model", IsIdentity = null, IsTitle = null, Order = 4, Component = null });
                applicationDbContext.SaveChanges();

                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigId", Label = "Config Id", IsIdentity = true, IsTitle = false, Order = 1, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsIdentity = false, IsTitle = false, Order = 2, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", IsIdentity = false, IsTitle = true, Order = 3, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Model", Label = "Model", IsIdentity = false, IsTitle = false, Order = 4, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ModelOptionItems" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ModelApi", Label = "Model Api", IsIdentity = false, IsTitle = false, Order = 5, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ControllerOptionItems" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Container", Label = "Container", IsIdentity = false, IsTitle = false, Order = 6, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ContainerOptionItems" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateTo", Label = "Navigate To", IsIdentity = false, IsTitle = false, Order = 7, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=PageOptionItems" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateToProperty", Label = "Navigate To Property", IsIdentity = false, IsTitle = false, Order = 8, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ModelFieldsOptionItems;Name=Model,Value=Config" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateToConfig", Label = "Navigate To Config", IsIdentity = false, IsTitle = false, Order = 9, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ConfigOptionItems" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateBack", Label = "Navigate Back", IsIdentity = false, IsTitle = false, Order = 10, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=PageOptionItems" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateBackProperty", Label = "Navigate Back Property", IsIdentity = false, IsTitle = false, Order = 11, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ModelFieldsOptionItems;Name=Model,Value=Config" });
                configConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateBackConfig", Label = "Navigate Back Config", IsIdentity = false, IsTitle = false, Order = 12, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ConfigOptionItems" });
                applicationDbContext.SaveChanges();

                configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigItemId", Label = "Config Item Id", IsIdentity = true, IsTitle = false, Order = 1, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
                configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PropertyName", Label = "PropertyName", IsIdentity = false, IsTitle = false, Order = 2, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ModelFieldsOptionItems;Name=Model,Value=Config" });
                configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Label", Label = "Label", IsIdentity = true, IsTitle = false, Order = 3, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
                configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "IsIdentity", Label = "IsIdentity", IsIdentity = true, IsTitle = false, Order = 4, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Checkbox, Headway.Razor.Controls", ComponentArgs = "Name=IsNullable,Value=True" });
                configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "IsTitle", Label = "IsTitle", IsIdentity = true, IsTitle = false, Order = 5, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Checkbox, Headway.Razor.Controls", ComponentArgs = "Name=IsNullable,Value=True" });
                configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", IsIdentity = true, IsTitle = false, Order = 6, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Integer, Headway.Razor.Controls", ComponentArgs = "Name=IsNullable,Value=False" });
                configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigContainer", Label = "Container", IsIdentity = false, IsTitle = false, Order = 7, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ContainerOptionItems" });
                configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Component", Label = "Component", IsIdentity = false, IsTitle = false, Order = 8, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ComponentOptionItems" });
                configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ComponentArgs", Label = "ComponentArgs", IsIdentity = false, IsTitle = false, Order = 9, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
                configItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigName", Label = "Config Name", IsIdentity = false, IsTitle = false, Order = 10, ConfigContainer = configConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = "Name=OptionsCode,Value=ConfigOptionItems" });
                applicationDbContext.SaveChanges();
            }
        }
    }
}
