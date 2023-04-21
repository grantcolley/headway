using Headway.Core.Constants;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Headway.SeedData
{
    public class DeveloperSeedData
    {
        private static ApplicationDbContext dbContext;

        private static Role developerRole;
        private static Dictionary<string, User> developers = new Dictionary<string, User>();

        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            dbContext = applicationDbContext;

            TruncateTables();

            CreateDemoModel();

            DemoModelsConfig();
            DemoModelConfig();
            DemoModelItemConfig();
            DemoModelItemsListDetailConfig();
            DemoModelTreeItemConfig();

            CreateDeveloperPermission();
            CreateDeveloperRole();
            AssignDeveloperRoleAllPermissions();
            CreateDevelopers();
        }

        private static void TruncateTables()
        {
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE DemoModelItems");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("TRUNCATE TABLE DemoModelTreeItems");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM DemoModels");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (DemoModels, RESEED, 1)");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DELETE FROM DemoModelComplexProperties");
            ((DbContext)dbContext).Database.ExecuteSqlRaw("DBCC CHECKIDENT (DemoModelComplexProperties, RESEED, 1)");
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
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Buttons.HEADER_BTN_IMAGE};Value={Buttons.BTN_ICON_ADD}|Name={Buttons.HEADER_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_CREATE}|Name={Buttons.ROW_BTN_IMAGE};Value={Buttons.BTN_ICON_EDIT_NOTE}|Name={Buttons.ROW_BTN_TOOLTIP};Value={Buttons.BTN_TOOLTIP_EDIT}",
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
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.TabDocument`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "DemoModels"
            };

            dbContext.Configs.Add(demoModelConfig);

            var demoModelContainer1 = new ConfigContainer { Name = "Model Components Div", Code = "MODEL_COMPONENTS_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Model Components", Order = 1 };
            var demoModelContainer2 = new ConfigContainer { Name = "List Component Div", Code = "LIST_COMPONENT_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "List Component", Order = 2 };
            var demoModelContainer3 = new ConfigContainer { Name = "Tree Component Div", Code = "TREE_COMPONENT_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Tree Component", Order = 3 };

            demoModelConfig.ConfigContainers.Add(demoModelContainer1);
            demoModelConfig.ConfigContainers.Add(demoModelContainer2);
            demoModelConfig.ConfigContainers.Add(demoModelContainer3);

            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelId", Label = "Demo Model Id", IsIdentity = true, Order = 1, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls", Tooltip = "Text" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Text", Label = "Text", IsTitle = true, Order = 2, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls", Tooltip = "Text" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "TextMultiline", Label = "TextMultiline", Order = 3, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.TextMultiline, Headway.Blazor.Controls", Tooltip = "TextMultiline", ComponentArgs = $"Name={Args.TEXT_MULTILINE_ROWS};Value=3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Integer", Label = "Integer", Order = 4, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.Integer, Headway.Blazor.Controls", Tooltip = "Integer" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Checkbox", Label = "Checkbox", Order = 5, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.Checkbox, Headway.Blazor.Controls", Tooltip = "Checkbox" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Decimal", Label = "Decimal", Order = 6, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.Decimal, Headway.Blazor.Controls", Tooltip = "Decimal" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OptionHorizontal", Label = "OptionHorizontal", Order = 7, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.OptionHorizontal, Headway.Blazor.Controls", Tooltip = "OptionHorizontal", ComponentArgs = $"Name=one;Value=Option 1|Name=two;Value=Option 2|Name=three;Value=Option 3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Date", Label = "Date", Order = 8, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.Date, Headway.Blazor.Controls", Tooltip = "Date" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "OptionVertical", Label = "OptionVertical", Order = 9, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.OptionVertical, Headway.Blazor.Controls", Tooltip = "OptionVertical", ComponentArgs = $"Name=one;Value=Option 1|Name=two;Value=Option 2|Name=three;Value=Option 3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Dropdown", Label = "Dropdown", Order = 10, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", Tooltip = "Dropdown", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.STATIC_OPTION_ITEMS}|Name=one;Value=Option 1|Name=two;Value=Option 2|Name=three;Value=Option 3" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DropdownComplex", Label = "DropdownComplex", Order = 11, ConfigContainer = demoModelContainer1, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.COMPLEX_OPTION_ITEMS}|Name={Options.DISPLAY_FIELD};Value=Name|Name={Args.MODEL};Value=Headway.Core.Model.DemoModelComplexProperty, Headway.Core|Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownComplex`1, Headway.Blazor.Controls" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelItems", Label = "Demo Model Items", Order = 12, ConfigContainer = demoModelContainer2, Component = "Headway.Blazor.Controls.Components.GenericField, Headway.Blazor.Controls", ConfigName = "DemoModelItem", ComponentArgs = $"Name={Args.LIST_CONFIG};Value=DemoModelItemsListDetail" });
            demoModelConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelTreeItems", Label = "Demo Model Tree Items", Order = 13, Tooltip = "Drag and drop items in a nested tree hierarchy", ConfigContainer = demoModelContainer3, Component = "Headway.Blazor.Controls.Components.GenericField, Headway.Blazor.Controls", ConfigName = "DemoModelTreeItem", ComponentArgs = $"Name={Args.UNIQUE_PROPERTY};Value={Args.CODE}|Name={Args.UNIQUE_PARENT_PROPERTY};Value={Args.CODE_PARENT}|Name={Args.LABEL_PROPERTY};Value=Name|Name={Args.LIST_PROPERTY};Value=DemoModelTreeItems" });

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
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.ListDetail`1, Headway.Blazor.Controls"
            };

            dbContext.Configs.Add(demoModelItemConfig);

            var demoModelItemConfigContainer = new ConfigContainer { Name = "Demo Model Item Div", Code = "DEMO_MODEL_ITEM_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Demo Model Item", Order = 1 };

            demoModelItemConfig.ConfigContainers.Add(demoModelItemConfigContainer);

            demoModelItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelItemId", Label = "Item Id", IsIdentity = true, Order = 1, ConfigContainer = demoModelItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            demoModelItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2, ConfigContainer = demoModelItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            demoModelItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 3, ConfigContainer = demoModelItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Integer, Headway.Blazor.Controls" });

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
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.TreeDetail`1, Headway.Blazor.Controls"
            };

            dbContext.Configs.Add(demoModelTreeItemConfig);

            var demoModelTreeItemConfigContainer = new ConfigContainer { Name = "Demo Model Tree Item Div", Code = "DEMO_MODEL_TREE_ITEM_DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Demo Model Tree Item", Order = 1 };

            demoModelTreeItemConfig.ConfigContainers.Add(demoModelTreeItemConfigContainer);

            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "DemoModelTreeItemId", Label = "Tree Item Id", IsIdentity = true, Order = 1, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ItemCode", Label = "Item Code", Order = 2, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 3, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            demoModelTreeItemConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Order", Label = "Order", Order = 4, ConfigContainer = demoModelTreeItemConfigContainer, Component = "Headway.Blazor.Controls.Components.Integer, Headway.Blazor.Controls" });

            dbContext.SaveChanges();
        }

        private static void CreateDeveloperPermission()
        {
            var developerPermission = new Permission { Name = HeadwayAuthorisation.DEVELOPER, Description = "Headway Developer" };
            dbContext.Permissions.Add(developerPermission);
            dbContext.SaveChanges();
        }

        private static void CreateDeveloperRole()
        {
            developerRole = new Role { Name = HeadwayAuthorisation.DEVELOPER, Description = "Headway Developer" };
            dbContext.Roles.Add(developerRole);
            dbContext.SaveChanges();
        }

        private static void AssignDeveloperRoleAllPermissions()
        {
            var permissions = dbContext.Permissions;
            developerRole.Permissions.AddRange(permissions);
            dbContext.SaveChanges();
        }

        private static void CreateDevelopers()
        {
            developers.Add("grant", new User { UserName = "grant", Email = "grant@email.com" });

            foreach(var developer in developers.Values)
            {
                developer.Roles.Add(developerRole);
                dbContext.Users.Add(developer);
            }

            dbContext.SaveChanges();
        }
    }
}
