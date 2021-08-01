using Headway.Core.Model;
using System.Linq;

namespace Headway.Repository.Data
{
    public class SeedData
    {
        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            if (!applicationDbContext.Users.Any()
                && !applicationDbContext.Permissions.Any())
            {
                var admin = new Permission { Name = "Admin", Description = "Administrator" };
                var user = new Permission { Name = "User", Description = "Headway User" };
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

                var permissionsConfig = new Config
                {
                    Name = "Permissions",
                    Title = "Permissions",
                    Model = "Permission",
                    ModelApi = "Permissions",
                    Container = "Table`1",
                    NavigateTo = "model",
                    NavigateToProperty = "PermissionId",
                    NavigateToConfig = "Permission",
                    NavigateBack = "list",
                    NavigateBackProperty = null,
                    NavigateBackConfig = "Permissions"
                };

                var permissionConfig = new Config
                {
                    Name = "Permission",
                    Title = "Permission",
                    Model = "Permission",
                    ModelApi = "Permissions",
                    Container = "Card`1",
                    NavigateTo = "list",
                    NavigateToProperty = null,
                    NavigateToConfig = "Permissions",
                    NavigateBack = "list",
                    NavigateBackProperty = null,
                    NavigateBackConfig = "Permissions"
                };

                var configsConfig = new Config
                {
                    Name = "Configs",
                    Title = "Configs",
                    Model = "Config",
                    ModelApi = "Configuration",
                    Container = "Table`1",
                    NavigateTo = "model",
                    NavigateToProperty = "ConfigId",
                    NavigateToConfig = "Config",
                    NavigateBack = "list",
                    NavigateBackProperty = null,
                    NavigateBackConfig = "Configs"
                };

                var configConfig = new Config
                {
                    Name = "Config",
                    Title = "Config",
                    Model = "Config",
                    ModelApi = "Configuration",
                    Container = "Card`1",
                    NavigateTo = "list",
                    NavigateToProperty = null,
                    NavigateToConfig = "Configs",
                    NavigateBack = "list",
                    NavigateBackProperty = null,
                    NavigateBackConfig = "Configs"
                };

                applicationDbContext.Configs.Add(configsConfig);
                applicationDbContext.Configs.Add(configConfig);
                applicationDbContext.Configs.Add(permissionsConfig);
                applicationDbContext.Configs.Add(permissionConfig);
                applicationDbContext.SaveChanges();

                permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", IsIdentity = null, IsTitle = null, Order = 1, Component = null });
                permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsIdentity = null, IsTitle = null, Order = 2, Component = null });
                permissionsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", IsIdentity = null, IsTitle = null, Order = 3, Component = null });
                applicationDbContext.SaveChanges();

                permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PermissionId", Label = "Permission Id", IsIdentity = true, IsTitle = false, Order = 1, Component = "Headway.Razor.Controls.Components.LabelData, Headway.Razor.Controls" });
                permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsIdentity = false, IsTitle = true, Order = 2, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                permissionConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", IsIdentity = false, IsTitle = false, Order = 3, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                applicationDbContext.SaveChanges();

                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigId", Label = "Config Id", IsIdentity = null, IsTitle = null, Order = 1, Component = null });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsIdentity = null, IsTitle = null, Order = 2, Component = null });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", IsIdentity = null, IsTitle = null, Order = 3, Component = null });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Model", Label = "Model", IsIdentity = null, IsTitle = null, Order = 4, Component = null });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ModelApi", Label = "Model Api", IsIdentity = null, IsTitle = null, Order = 5, Component = null });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Container", Label = "Container", IsIdentity = null, IsTitle = null, Order = 6, Component = null });
                applicationDbContext.SaveChanges();

                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ConfigId", Label = "Config Id", IsIdentity = true, IsTitle = false, Order = 1, Component = "Headway.Razor.Controls.Components.LabelData, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsIdentity = false, IsTitle = false, Order = 2, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", IsIdentity = false, IsTitle = true, Order = 3, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Model", Label = "Model", IsIdentity = false, IsTitle = false, Order = 4, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ModelApi", Label = "Model Api", IsIdentity = false, IsTitle = false, Order = 5, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Container", Label = "Container", IsIdentity = false, IsTitle = false, Order = 6, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateTo", Label = "Navigate To", IsIdentity = false, IsTitle = false, Order = 7, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateToProperty", Label = "Navigate To Property", IsIdentity = false, IsTitle = false, Order = 8, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateToConfig", Label = "Navigate To Config", IsIdentity = false, IsTitle = false, Order = 9, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateBack", Label = "Navigate Back", IsIdentity = false, IsTitle = false, Order = 10, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateBackProperty", Label = "Navigate Back Property", IsIdentity = false, IsTitle = false, Order = 11, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                configsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "NavigateBackConfig", Label = "Navigate Back Config", IsIdentity = false, IsTitle = false, Order = 12, Component = "Headway.Razor.Controls.Components.LabelText, Headway.Razor.Controls" });
                applicationDbContext.SaveChanges();
            }
        }
    }
}
