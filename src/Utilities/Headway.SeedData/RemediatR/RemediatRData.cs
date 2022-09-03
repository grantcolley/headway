using Headway.Core.Constants;
using Headway.Core.Model;
using Headway.RemediatR.Core.Constants;
using Headway.RemediatR.Core.Enums;
using Headway.RemediatR.Core.Model;
using Headway.RemediatR.Repository.Constants;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Headway.SeedData.RemediatR
{
    public class RemediatRData
    {
        private static ApplicationDbContext dbContext;

        private readonly static Dictionary<string, Permission> permissions = new();
        private readonly static Dictionary<string, Role> roles = new();
        private readonly static Dictionary<string, User> users = new();

        public static void Initialise(ApplicationDbContext applicationDbContext)
        {
            dbContext = applicationDbContext;

            TruncateTables();

            CreateCountries();
            CreatePermissions();
            CreateRoles();
            CreateUsers();
            AssignUsersRoles();
            CreateNavigation();
            CreatePrograms();
            CreateCustomers();

            ProgramsConfig();
            ProgramConfig();
            CustomersConfig();
            CustomerConfig();
            ProductConfig();
            ProductsListDetailConfig();
            RedressCasesConfig();
            NewRedressCasesConfig();
            RedressConfig();
            RedressCustomerConfig();
            RedressProductConfig();
        }

        private static void TruncateTables()
        {
            dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE Countries");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Programs");
            dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT (Programs, RESEED, 1)");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Products");
            dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT (Products, RESEED, 1)");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Customers");
            dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT (Customers, RESEED, 1)");
            dbContext.Database.ExecuteSqlRaw("DELETE FROM Redresses");
            dbContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT (Redresses, RESEED, 1)");
        }

        private static void CreateCountries()
        {
            List<Country> countries = new();

            var lines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RemediatR", "_countries.csv"));

            foreach (var line in lines.Skip(1))
            {
                var c = line.Split(',');

                countries.Add(new Country 
                {
                    Code = c[0], 
                    Latitude = string.IsNullOrWhiteSpace(c[1]) ? default(decimal?) : decimal.Parse(c[1]), 
                    Longitude = string.IsNullOrWhiteSpace(c[2]) ? default(decimal?) : decimal.Parse(c[2]), 
                    Name = c[3] 
                });
            }

            foreach (var country in countries)
            {
                dbContext.Countries.Add(country);
            }

            dbContext.SaveChanges();
        }

        private static void CreatePermissions()
        {
            permissions.Add(RemediatRAuthorisation.CUSTOMER_READ, new Permission { Name = RemediatRAuthorisation.CUSTOMER_READ, Description = "RemediatR Customer Read" });
            permissions.Add(RemediatRAuthorisation.CUSTOMER_WRITE, new Permission { Name = RemediatRAuthorisation.CUSTOMER_WRITE, Description = "RemediatR Customer Write" });

            permissions.Add(RemediatRAuthorisation.REDRESS_READ, new Permission { Name = RemediatRAuthorisation.REDRESS_READ, Description = "RemediatR Redress Read" });
            permissions.Add(RemediatRAuthorisation.REDRESS_WRITE, new Permission { Name = RemediatRAuthorisation.REDRESS_WRITE, Description = "RemediatR Redress Write" });
            permissions.Add(RemediatRAuthorisation.REDRESS_TRANSITION, new Permission { Name = RemediatRAuthorisation.REDRESS_TRANSITION, Description = "RemediatR Redress Transition" });
            permissions.Add(RemediatRAuthorisation.COMMUNICATION_DISPATCH_TRANSITION, new Permission { Name = RemediatRAuthorisation.COMMUNICATION_DISPATCH_TRANSITION, Description = "RemediatR Communication Dispatch Transition" });
            permissions.Add(RemediatRAuthorisation.AWAITING_REPONSE_TRANSITION, new Permission { Name = RemediatRAuthorisation.AWAITING_REPONSE_TRANSITION, Description = "RemediatR Awaiting Response Transition" });

            permissions.Add(RemediatRAuthorisation.REDRESS_REVIEW_TRANSITION, new Permission { Name = RemediatRAuthorisation.REDRESS_REVIEW_TRANSITION, Description = "RemediatR Redress Refund Review Transition" });
            permissions.Add(RemediatRAuthorisation.REDRESS_COMPLETE, new Permission { Name = RemediatRAuthorisation.REDRESS_COMPLETE, Description = "RemediatR Redress Complete" });

            permissions.Add(RemediatRAuthorisation.REFUND_READ, new Permission { Name = RemediatRAuthorisation.REFUND_READ, Description = "RemediatR Refund Read" });
            permissions.Add(RemediatRAuthorisation.REFUND_WRITE, new Permission { Name = RemediatRAuthorisation.REFUND_WRITE, Description = "RemediatR Refund Write" });
            permissions.Add(RemediatRAuthorisation.REFUND_CACULATION_COMPLETE, new Permission { Name = RemediatRAuthorisation.REFUND_CACULATION_COMPLETE, Description = "RemediatR Refund Calculation Complete" });
            permissions.Add(RemediatRAuthorisation.REFUND_VERIFICATION_COMPLETE, new Permission { Name = RemediatRAuthorisation.REFUND_VERIFICATION_COMPLETE, Description = "RemediatR Refund Varification Complete" });

            permissions.Add(RemediatRAuthorisation.REFUND_REVIEW_TRANSITION, new Permission { Name = RemediatRAuthorisation.REFUND_REVIEW_TRANSITION, Description = "RemediatR Refund Review Transition" });

            foreach (var permission in permissions.Values)
            {
                dbContext.Permissions.Add(permission);
            }

            dbContext.SaveChanges();
        }

        private static void CreateRoles()
        {
            roles.Add(RemediatRAuthorisation.REDRESS_CASE_OWNER, new Role { Name = RemediatRAuthorisation.REDRESS_CASE_OWNER, Description = "RemediatR Redress Case Owner" });
            roles.Add(RemediatRAuthorisation.REDRESS_REVIEWER, new Role { Name = RemediatRAuthorisation.REDRESS_REVIEWER, Description = "RemediatR Redress Reviewer" });
            roles.Add(RemediatRAuthorisation.REFUND_ASSESSOR, new Role { Name = RemediatRAuthorisation.REFUND_ASSESSOR, Description = "RemediatR Refund Assessor" });
            roles.Add(RemediatRAuthorisation.REFUND_REVIEWER, new Role { Name = RemediatRAuthorisation.REFUND_REVIEWER, Description = "RemediatR Refund Reviewer" });

            foreach (var role in roles.Values)
            {
                dbContext.Roles.Add(role);
            }

            roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(permissions[RemediatRAuthorisation.CUSTOMER_READ]);
            roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(permissions[RemediatRAuthorisation.CUSTOMER_WRITE]);
            roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(permissions[RemediatRAuthorisation.REDRESS_READ]);
            roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(permissions[RemediatRAuthorisation.REDRESS_WRITE]);
            roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(permissions[RemediatRAuthorisation.REDRESS_TRANSITION]);
            roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(permissions[RemediatRAuthorisation.REFUND_READ]);
            roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(permissions[RemediatRAuthorisation.COMMUNICATION_DISPATCH_TRANSITION]);
            roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(permissions[RemediatRAuthorisation.AWAITING_REPONSE_TRANSITION]);

            roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(permissions[RemediatRAuthorisation.CUSTOMER_READ]);
            roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(permissions[RemediatRAuthorisation.REDRESS_READ]);
            roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(permissions[RemediatRAuthorisation.REFUND_READ]);
            roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(permissions[RemediatRAuthorisation.REDRESS_REVIEW_TRANSITION]);
            roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(permissions[RemediatRAuthorisation.REDRESS_COMPLETE]);

            roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(permissions[RemediatRAuthorisation.REFUND_READ]);
            roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(permissions[RemediatRAuthorisation.REFUND_WRITE]);
            roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(permissions[RemediatRAuthorisation.REDRESS_READ]);
            roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(permissions[RemediatRAuthorisation.REFUND_CACULATION_COMPLETE]);
            roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(permissions[RemediatRAuthorisation.REFUND_VERIFICATION_COMPLETE]);

            roles[RemediatRAuthorisation.REFUND_REVIEWER].Permissions.Add(permissions[RemediatRAuthorisation.REFUND_READ]);
            roles[RemediatRAuthorisation.REFUND_REVIEWER].Permissions.Add(permissions[RemediatRAuthorisation.REDRESS_READ]);
            roles[RemediatRAuthorisation.REFUND_REVIEWER].Permissions.Add(permissions[RemediatRAuthorisation.REFUND_REVIEW_TRANSITION]);

            dbContext.SaveChanges();
        }

        private static void CreateUsers()
        {
            users.Add("grace", new User { UserName = "grace", Email = "grace@email.com" });
            users.Add("mel", new User { UserName = "mel", Email = "mel@email.com" });
            users.Add("bill", new User { UserName = "bill", Email = "bill@email.com" });
            users.Add("will", new User { UserName = "will", Email = "will@email.com" });
            users.Add("mary", new User { UserName = "mary", Email = "mary@email.com" });

            foreach (var user in users.Values)
            {
                dbContext.Users.Add(user);
            }

            dbContext.SaveChanges();
        }

        private static void AssignUsersRoles()
        {
            users["grace"].Roles.Add(roles[RemediatRAuthorisation.REDRESS_CASE_OWNER]);
            users["mel"].Roles.Add(roles[RemediatRAuthorisation.REDRESS_REVIEWER]);
            users["bill"].Roles.Add(roles[RemediatRAuthorisation.REFUND_ASSESSOR]);
            users["will"].Roles.Add(roles[RemediatRAuthorisation.REFUND_ASSESSOR]);
            users["mary"].Roles.Add(roles[RemediatRAuthorisation.REFUND_REVIEWER]);

            dbContext.SaveChanges();
        }

        private static void CreateNavigation()
        {
            var remediatR = new Module { Name = "RemediatR", Icon = "Balance", Order = 1, Permission = RemediatRAuthorisation.REDRESS_READ };

            dbContext.Modules.Add(remediatR);

            var customerCatgory = new Category { Name = "Customer", Icon = "PersonOutline", Order = 1, Permission = RemediatRAuthorisation.CUSTOMER_READ };
            var redressCatgory = new Category { Name = "Redress", Icon = "ListAlt", Order = 2, Permission = RemediatRAuthorisation.REDRESS_READ };
            var programCatgory = new Category { Name = "Program", Icon = "Apps", Order = 3, Permission = HeadwayAuthorisation.ADMIN };

            dbContext.Categories.Add(customerCatgory);
            dbContext.Categories.Add(redressCatgory);
            dbContext.Categories.Add(programCatgory);

            var customersMenuItem = new MenuItem { Name = "Customers", Icon = "PeopleOutline", NavigatePage = "Page", Order = 1, Permission = RemediatRAuthorisation.CUSTOMER_READ, Config = "Customers" };
            var redressCasesMenuItem = new MenuItem { Name = "Redress Cases", Icon = "List", NavigatePage = "Page", Order = 1, Permission = RemediatRAuthorisation.REDRESS_READ, Config = RemediatRSearchSource.REDRESSCASES };
            var createRedressCasesMenuItem = new MenuItem { Name = "New Redress Case", Icon = "PlaylistAdd", NavigatePage = "Page", Order = 1, Permission = RemediatRAuthorisation.REDRESS_READ, Config = RemediatRSearchSource.NEW_REDRESS_CASE };
            var programsMenuItem = new MenuItem { Name = "Programs", Icon = "AppRegistration", NavigatePage = "Page", Order = 1, Permission = HeadwayAuthorisation.ADMIN, Config = "Programs" };

            dbContext.MenuItems.Add(customersMenuItem);
            dbContext.MenuItems.Add(redressCasesMenuItem);
            dbContext.MenuItems.Add(createRedressCasesMenuItem);
            dbContext.MenuItems.Add(programsMenuItem);

            customerCatgory.MenuItems.Add(customersMenuItem);
            redressCatgory.MenuItems.Add(redressCasesMenuItem);
            redressCatgory.MenuItems.Add(createRedressCasesMenuItem);
            programCatgory.MenuItems.Add(programsMenuItem);

            remediatR.Categories.Add(customerCatgory);
            remediatR.Categories.Add(redressCatgory);
            remediatR.Categories.Add(programCatgory);

            dbContext.SaveChanges();
        }

        private static void CreatePrograms()
        {
            Dictionary<string, Program> programs = new();

            programs.Add(
                "IRMS",
                new Program
                {
                    Name = "IRMS",
                    Description = "Interest Rate Missold",
                    Compensation = 400.00m,
                    CompensatoryInterest = 5.00m,
                    StartDate = DateTime.Today.Subtract(TimeSpan.FromDays(300)),
                    ProductType = ProductType.Vehicle,
                    RateType = RateType.Fixed,
                    RepaymentType = RepaymentType.Repayment
                });

            programs.Add(
                "BTL-IOC",
                new Program
                {
                    Name = "BTL-RP",
                    Description = "Buy To Let Mortgage Redemption Penalty",
                    Compensation = 1500.00m,
                    CompensatoryInterest = 7.50m,
                    StartDate = DateTime.Today.Subtract(TimeSpan.FromDays(200)),
                    ProductType = ProductType.Home,
                    RateType = RateType.Variable,
                    RepaymentType = RepaymentType.InterestOnly
                });

            programs.Add(
                "STULIOC",
                new Program
                {
                    Name = "STULIOC",
                    Description = "Student Loan Introductory Offer Conversion",
                    Compensation = 275.00m,
                    CompensatoryInterest = 3.75m,
                    StartDate = DateTime.Today.Subtract(TimeSpan.FromDays(450)),
                    ProductType = ProductType.Student,
                    RateType = RateType.Tracker,
                    RepaymentType = RepaymentType.Repayment
                });

            foreach (var program in programs.Values)
            {
                dbContext.Programs.Add(program);
            }

            dbContext.SaveChanges();
        }

        private static void CreateCustomers()
        {
            List<Customer> customers = new();

            var customerLines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RemediatR", "_customers.csv"));
            var productLines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RemediatR", "_products.csv"));

            foreach (var customerLine in customerLines.Skip(1))
            {
                var c = customerLine.Split(',');

                var customer = new Customer
                {
                    Title = c[0],
                    FirstName = c[1],
                    Surname = c[2],
                    Address1 = c[3],
                    Address2 = c[4],
                    PostCode = c[5],
                    Country = c[6],
                    Telephone = c[7],
                    Email = c[8],
                    SortCode = c[9],
                    AccountNumber = c[10],
                    AccountStatus = AccountStatus.Active
                };

                var products = productLines.Where(p => p.StartsWith($"{c[0]} {c[1]} {c[2]}"));

                foreach (var product in products)
                {
                    var p = product.Split(",");

                    customer.Products.Add(new Product
                    {
                        Name = p[1],
                        ProductType = (ProductType)Enum.Parse(typeof(ProductType), p[2]),
                        RateType = (RateType)Enum.Parse(typeof(RateType), p[3]),
                        RepaymentType = (RepaymentType)Enum.Parse(typeof(RepaymentType), p[4]),
                        Duration = int.Parse(p[5]),
                        Rate = decimal.Parse(p[6]),
                        StartDate = DateTime.Parse(p[7]),
                        Value = decimal.Parse(p[8])
                    });
                }

                customers.Add(customer);

                dbContext.Customers.Add(customer);
            }

            dbContext.SaveChanges();
        }

        private static void ProgramsConfig()
        {
            var programsConfig = new Config
            {
                Name = "Programs",
                Title = "Programs",
                Description = "List of RemediatR programs",
                Model = "Headway.RemediatR.Core.Model.Program, Headway.RemediatR.Core",
                ModelApi = "RemediatRProgram",
                OrderModelBy = "Name",
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
                DocumentArgs = $"Name={Css.HEADER_BTN_IMAGE};Value={Css.BTN_IMAGE_PLUS}|Name={Css.HEADER_BTN_TOOLTIP};Value={Css.BTN_TOOLTIP_CREATE}|Name={Css.ROW_BTN_IMAGE};Value={Css.BTN_IMAGE_EDIT}|Name={Css.ROW_BTN_TOOLTIP};Value={Css.BTN_TOOLTIP_EDIT}",
                NavigatePage = "Page",
                NavigateProperty = "ProgramId",
                NavigateConfig = "Program",
                NavigateResetBreadcrumb = true
            };

            dbContext.Configs.Add(programsConfig);

            programsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProgramId", Label = "Program Id", Order = 1 });
            programsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            programsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3 });

            dbContext.SaveChanges();
        }

        private static void ProgramConfig()
        {
            var programConfig = new Config
            {
                Name = "Program",
                Title = "Program",
                Description = "Create, update or delete a RemediatR program",
                Model = "Headway.RemediatR.Core.Model.Program, Headway.RemediatR.Core",
                ModelApi = "RemediatRProgram",
                CreateLocal = true,
                Document = "Headway.Razor.Controls.Documents.TabDocument`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Programs"
            };

            dbContext.Configs.Add(programConfig);

            var programConfigContainer = new ConfigContainer { Name = "Program Div", Code = "PROGRAM DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Program", Order = 1 };

            programConfig.ConfigContainers.Add(programConfigContainer);

            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProgramId", Label = "Program Id", IsIdentity = true, Order = 1, ConfigContainer = programConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = programConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3, ConfigContainer = programConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Compensation", Label = "Compensation", Order = 4, ConfigContainer = programConfigContainer, Component = "Headway.Razor.Controls.Components.DecimalNullable, Headway.Razor.Controls", ComponentArgs = $"Name={Args.FORMAT};Value={Args.FORMAT_F2}|Name={Args.MAX};Value=9999999999999.99" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CompensatoryInterest", Label = "Compensatory Interest", Order = 5, ConfigContainer = programConfigContainer, Component = "Headway.Razor.Controls.Components.DecimalNullable, Headway.Razor.Controls", ComponentArgs = $"Name={Args.FORMAT};Value={Args.FORMAT_F2}|Name={Args.MAX_LENGTH};Value=5|Name={Args.MAX};Value=99.99" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StartDate", Label = "Start Date", Order = 6, ConfigContainer = programConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "EndDate", Label = "End Date", Order = 7, ConfigContainer = programConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductType", Label = "ProductType", Order = 8, ConfigContainer = programConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.ProductType, Headway.RemediatR.Core" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RateType", Label = "RateType", Order = 9, ConfigContainer = programConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.RateType, Headway.RemediatR.Core" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RepaymentType", Label = "RepaymentType", Order = 10, ConfigContainer = programConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.RepaymentType, Headway.RemediatR.Core" });

            dbContext.SaveChanges();
        }

        private static void CustomersConfig()
        {
            var customersConfig = new Config
            {
                Name = "Customers",
                Title = "Customers",
                Description = "List of RemediatR customers",
                Model = "Headway.RemediatR.Core.Model.Customer, Headway.RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                OrderModelBy = "Surname",
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
                DocumentArgs = $"Name={Css.HEADER_BTN_IMAGE};Value={Css.BTN_IMAGE_PLUS}|Name={Css.HEADER_BTN_TOOLTIP};Value={Css.BTN_TOOLTIP_CREATE}|Name={Css.ROW_BTN_IMAGE};Value={Css.BTN_IMAGE_EDIT}|Name={Css.ROW_BTN_TOOLTIP};Value={Css.BTN_TOOLTIP_EDIT}",
                SearchComponent = "Headway.Razor.Controls.SearchComponents.StandardSearchComponent, Headway.Razor.Controls",
                UseSearchComponent = true,
                NavigatePage = "Page",
                NavigateProperty = "CustomerId",
                NavigateConfig = "Customer",
                NavigateResetBreadcrumb = true
            };

            customersConfig.ConfigSearchItems.AddRange(new List<ConfigSearchItem>
            {
                new ConfigSearchItem
                {
                    Label = "Customer Id",
                    ParameterName = "CustomerId",
                    Tooltip = "The customer identifier",
                    Component = "Headway.Razor.Controls.SearchComponents.SearchText, Headway.Razor.Controls",
                    Order = 1
                },
                new ConfigSearchItem
                {
                    Label = "Surname",
                    ParameterName = "Surname",
                    Tooltip = "The customer surname",
                    Component = "Headway.Razor.Controls.SearchComponents.SearchText, Headway.Razor.Controls",
                    Order = 2
                }
            });

            dbContext.Configs.Add(customersConfig);

            customersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CustomerId", Label = "Customer Id", Order = 1 });
            customersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", Order = 2 });
            customersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FirstName", Label = "First Name", Order = 3 });
            customersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Surname", Label = "Surname", Order = 4 });

            dbContext.SaveChanges();
        }

        private static void CustomerConfig()
        {
            var customerConfig = new Config
            {
                Name = "Customer",
                Title = "Customer",
                Description = "Create, update or delete a RemediatR customer",
                Model = "Headway.RemediatR.Core.Model.Customer, Headway.RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                CreateLocal = true,
                Document = "Headway.Razor.Controls.Documents.TabDocument`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Customers"
            };

            dbContext.Configs.Add(customerConfig);

            var customerConfigContainer = new ConfigContainer { Name = "Customer Div", Code = "CUSTOMER DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Customer", Order = 1 };
            var productsConfigContainer = new ConfigContainer { Name = "Products Div", Code = "PRODUCTS DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Products", Order = 2 };

            customerConfig.ConfigContainers.Add(customerConfigContainer);
            customerConfig.ConfigContainers.Add(productsConfigContainer);

            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CustomerId", Label = "Customer Id", IsIdentity = true, Order = 1, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", Order = 2, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.STATIC_OPTION_ITEMS}|Name=Ms;Value=Ms|Name=Mr;Value=Mr|Name=Mrs;Value=Mrs" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FirstName", Label = "FirstName", Order = 3, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Surname", Label = "Surname", IsTitle = true, Order = 4, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AccountStatus", Label = "Account Status", Order = 5, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.AccountStatus, Headway.RemediatR.Core" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Telephone", Label = "Telephone", Order = 6, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Email", Label = "Email", Order = 7, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "SortCode", Label = "Sort Code", Order = 8, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AccountNumber", Label = "Account Number", Order = 9, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address1", Label = "Address1", Order = 10, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address2", Label = "Address2", Order = 11, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address3", Label = "Address3", Order = 12, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address4", Label = "Address4", Order = 13, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address5", Label = "Address5", Order = 14, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Country", Label = "Country", Order = 15, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.COUNTRY_OPTION_ITEMS}" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PostCode", Label = "PostCode", Order = 16, ConfigContainer = customerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Products", Label = "Products", Order = 17, ConfigContainer = productsConfigContainer, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "Product", ComponentArgs = $"Name={Args.LIST_CONFIG};Value=ProductsListDetail" });

            dbContext.SaveChanges();
        }

        private static void ProductConfig()
        {
            var productConfig = new Config
            {
                Name = "Product",
                Title = "Product",
                Description = "Product sold to the customer",
                Model = "Headway.RemediatR.Core.Model.Product, Headway.RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                CreateLocal = true,
                Document = "Headway.Razor.Controls.Documents.ListDetail`1, Headway.Razor.Controls"
            };

            dbContext.Configs.Add(productConfig);

            var productConfigContainer = new ConfigContainer { Name = "Product Div", Code = "PRODUCT DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Product", Order = 1 };

            productConfig.ConfigContainers.Add(productConfigContainer);

            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductId", Label = "Product Id", IsIdentity = true, Order = 1, ConfigContainer = productConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = productConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Value", Label = "Value", Order = 3, ConfigContainer = productConfigContainer, Component = "Headway.Razor.Controls.Components.DecimalNullable, Headway.Razor.Controls", ComponentArgs = $"Name={Args.FORMAT};Value={Args.FORMAT_F2}|Name={Args.MAX};Value=9999999999999.99" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Rate", Label = "Rate", Order = 4, ConfigContainer = productConfigContainer, Component = "Headway.Razor.Controls.Components.DecimalNullable, Headway.Razor.Controls", ComponentArgs = $"Name={Args.FORMAT};Value={Args.FORMAT_F2}|Name={Args.MAX_LENGTH};Value=5|Name={Args.MAX};Value=99.99" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Duration", Label = "Duration", Order = 5, ConfigContainer = productConfigContainer, Component = "Headway.Razor.Controls.Components.IntegerNullable, Headway.Razor.Controls", ComponentArgs = $"Name={Args.MIN};Value=3|Name={Args.MAX};Value=360", Tooltip = "Duration must be min 3 and max 360" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StartDate", Label = "Start Date", Order = 6, ConfigContainer = productConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductType", Label = "ProductType", Order = 7, ConfigContainer = productConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.ProductType, Headway.RemediatR.Core" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RateType", Label = "RateType", Order = 8, ConfigContainer = productConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.RateType, Headway.RemediatR.Core" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RepaymentType", Label = "RepaymentType", Order = 9, ConfigContainer = productConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.RepaymentType, Headway.RemediatR.Core" });

            dbContext.SaveChanges();
        }

        private static void ProductsListDetailConfig()
        {
            var productsListDetailConfig = new Config
            {
                Name = "ProductsListDetail",
                Title = "ProductsListDetail",
                Description = "List of products sold to the customer",
                Model = "Headway.RemediatR.Core.Model.Product, Headway.RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                OrderModelBy = "StartDate"
            };

            dbContext.Configs.Add(productsListDetailConfig);

            productsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Product Name", Order = 1 });
            productsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StartDate", Label = "Start Date", Order = 2 });
            productsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Duration", Label = "Duration", Order = 3 });
            productsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Value", Label = "Value", Order = 4 });

            dbContext.SaveChanges();
        }

        private static void RedressCustomerConfig()
        {
            var redressCustomerConfig = new Config
            {
                Name = "RedressCustomer",
                Title = "Customer",
                Description = "RemediatR customer",
                Model = "Headway.RemediatR.Core.Model.Customer, Headway.RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                Document = "Headway.Razor.Controls.Documents.Content`1, Headway.Razor.Controls"
            };

            dbContext.Configs.Add(redressCustomerConfig);

            var redressCustomerConfigContainer = new ConfigContainer { Name = "Redress Customer Div", Code = "REDRESS CUSTOMER DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Customer", Order = 1 };

            redressCustomerConfig.ConfigContainers.Add(redressCustomerConfigContainer);

            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CustomerId", Label = "Customer Id", IsIdentity = true, Order = 1, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", Order = 2, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.STATIC_OPTION_ITEMS}|Name=Ms;Value=Ms|Name=Mr;Value=Mr|Name=Mrs;Value=Mrs" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FirstName", Label = "FirstName", Order = 3, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Surname", Label = "Surname", IsTitle = true, Order = 4, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AccountStatus", Label = "Account Status", Order = 5, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.AccountStatus, Headway.RemediatR.Core" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Telephone", Label = "Telephone", Order = 6, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Email", Label = "Email", Order = 7, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "SortCode", Label = "Sort Code", Order = 8, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AccountNumber", Label = "Account Number", Order = 9, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address1", Label = "Address1", Order = 10, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address2", Label = "Address2", Order = 11, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address3", Label = "Address3", Order = 12, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address4", Label = "Address4", Order = 13, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address5", Label = "Address5", Order = 14, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Country", Label = "Country", Order = 15, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Dropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.COUNTRY_OPTION_ITEMS}" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PostCode", Label = "PostCode", Order = 16, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });

            dbContext.SaveChanges();
        }

        private static void RedressProductConfig()
        {
            var redressProductConfig = new Config
            {
                Name = "RedressProduct",
                Title = "Product",
                Description = "Redress product sold to the customer",
                Model = "Headway.RemediatR.Core.Model.Product, Headway.RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                Document = "Headway.Razor.Controls.Documents.Content`1, Headway.Razor.Controls"
            };

            dbContext.Configs.Add(redressProductConfig);

            var redressProductConfigContainer = new ConfigContainer { Name = "Redress Product Div", Code = "REDRESS PRODUCT DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Product", Order = 1 };

            redressProductConfig.ConfigContainers.Add(redressProductConfigContainer);

            redressProductConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductId", Label = "Product Id", IsIdentity = true, Order = 1, ConfigContainer = redressProductConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressProductConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = redressProductConfigContainer, Component = "Headway.Razor.Controls.Components.Text, Headway.Razor.Controls" });
            redressProductConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Value", Label = "Value", Order = 3, ConfigContainer = redressProductConfigContainer, Component = "Headway.Razor.Controls.Components.DecimalNullable, Headway.Razor.Controls", ComponentArgs = $"Name={Args.FORMAT};Value={Args.FORMAT_F2}|Name={Args.MAX};Value=9999999999999.99" });
            redressProductConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Rate", Label = "Rate", Order = 4, ConfigContainer = redressProductConfigContainer, Component = "Headway.Razor.Controls.Components.DecimalNullable, Headway.Razor.Controls", ComponentArgs = $"Name={Args.FORMAT};Value={Args.FORMAT_F2}|Name={Args.MAX_LENGTH};Value=5|Name={Args.MAX};Value=99.99" });
            redressProductConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Duration", Label = "Duration", Order = 5, ConfigContainer = redressProductConfigContainer, Component = "Headway.Razor.Controls.Components.IntegerNullable, Headway.Razor.Controls", ComponentArgs = $"Name={Args.MIN};Value=3|Name={Args.MAX};Value=360", Tooltip = "Duration must be min 3 and max 360" });
            redressProductConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StartDate", Label = "Start Date", Order = 6, ConfigContainer = redressProductConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });
            redressProductConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductType", Label = "ProductType", Order = 7, ConfigContainer = redressProductConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.ProductType, Headway.RemediatR.Core" });
            redressProductConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RateType", Label = "RateType", Order = 8, ConfigContainer = redressProductConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.RateType, Headway.RemediatR.Core" });
            redressProductConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RepaymentType", Label = "RepaymentType", Order = 9, ConfigContainer = redressProductConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownEnum`1, Headway.Razor.Controls|Name={Args.MODEL};Value=Headway.RemediatR.Core.Enums.RepaymentType, Headway.RemediatR.Core" });

            dbContext.SaveChanges();
        }

        private static void RedressCasesConfig()
        {
            var redressCasesConfig = new Config
            {
                Name = RemediatRSearchSource.REDRESSCASES,
                Title = "Redress Cases",
                Description = "List of RemediatR redress cases",
                Model = "Headway.RemediatR.Core.Model.RedressCase, Headway.RemediatR.Core",
                ModelApi = "RemediatRRedress",
                OrderModelBy = "CustomerName",
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
                DocumentArgs = $"Name={Css.ROW_BTN_IMAGE};Value={Css.BTN_IMAGE_EDIT}|Name={Css.ROW_BTN_TOOLTIP};Value={Css.BTN_TOOLTIP_EDIT}",
                SearchComponent = "Headway.Razor.Controls.SearchComponents.StandardSearchComponent, Headway.Razor.Controls",
                UseSearchComponent = true,
                NavigatePage = "Page",
                NavigateProperty = "RedressId",
                NavigateConfig = "Redress",
                NavigateResetBreadcrumb = true
            };

            redressCasesConfig.ConfigSearchItems.AddRange(new List<ConfigSearchItem>
            {
                new ConfigSearchItem
                {
                    Label = "Program",
                    ParameterName = "Name",
                    Tooltip = "The redress program",
                    Component = "Headway.Razor.Controls.SearchComponents.SearchDropdown, Headway.Razor.Controls",
                    ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={RemediatROptions.PROGRAMS_OPTION_ITEMS}|Name={Args.STYLE};Value=min-width:150px",
                    Order = 1
                },
                new ConfigSearchItem
                {
                    Label = "Customer Id",
                    ParameterName = "CustomerId",
                    Tooltip = "The customer identifier",
                    Component = "Headway.Razor.Controls.SearchComponents.SearchText, Headway.Razor.Controls",
                    Order = 2
                },
                new ConfigSearchItem
                {
                    Label = "Surname",
                    ParameterName = "Surname",
                    Tooltip = "The customer surname",
                    Component = "Headway.Razor.Controls.SearchComponents.SearchText, Headway.Razor.Controls",
                    Order = 3
                }
            });

            dbContext.Configs.Add(redressCasesConfig);

            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressId", Label = "Redress Id", Order = 1 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CustomerName", Label = "CustomerName", Order = 2 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProgramName", Label = "ProgramName", Order = 3 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductName", Label = "ProductName", Order = 4 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Status", Label = "Status", Order = 5 });

            dbContext.SaveChanges();
        }

        private static void NewRedressCasesConfig()
        {
            var redressCasesConfig = new Config
            {
                Name = RemediatRSearchSource.NEW_REDRESS_CASE,
                Title = "New Redress Case",
                Description = "Create a new RemediatR redress case",
                Model = "Headway.RemediatR.Core.Model.NewRedressCase, Headway.RemediatR.Core",
                ModelApi = "RemediatRRedress",
                OrderModelBy = "CustomerId",
                Document = "Headway.Razor.Controls.Documents.Table`1, Headway.Razor.Controls",
                DocumentArgs = $"Name={Css.ROW_BTN_IMAGE};Value={Css.BTN_IMAGE_PLUS}|Name={Css.ROW_BTN_TOOLTIP};Value={Css.BTN_TOOLTIP_CREATE}",
                SearchComponent = "Headway.Razor.Controls.SearchComponents.StandardSearchComponent, Headway.Razor.Controls",
                UseSearchComponent = true,
                NavigatePage = "Page",
                NavigateConfig = "Redress",
                NavigateResetBreadcrumb = true
            };

            redressCasesConfig.ConfigSearchItems.AddRange(new List<ConfigSearchItem>
            {
                new ConfigSearchItem
                {
                    Label = "Product Type",
                    ParameterName = "ProductType",
                    Tooltip = "The type of product in scope for redress",
                    Component = "Headway.Razor.Controls.SearchComponents.SearchDropdown, Headway.Razor.Controls",
                    ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.ENUM_NAMES_OPTION_ITEMS}|Name={Args.TYPE};Value=Headway.RemediatR.Core.Enums.ProductType, Headway.RemediatR.Core|Name={Args.STYLE};Value=min-width:150px",
                    Order = 1
                },
                new ConfigSearchItem
                {
                    Label = "Customer Id",
                    ParameterName = "CustomerId",
                    Tooltip = "The customer identifier",
                    Component = "Headway.Razor.Controls.SearchComponents.SearchText, Headway.Razor.Controls",
                    Order = 2
                },
                new ConfigSearchItem
                {
                    Label = "Surname",
                    ParameterName = "Surname",
                    Tooltip = "The customer surname",
                    Component = "Headway.Razor.Controls.SearchComponents.SearchText, Headway.Razor.Controls",
                    Order = 3
                }
            });

            dbContext.Configs.Add(redressCasesConfig);

            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressId", Label = "Redress Id", Order = 1 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProgramName", Label = "Program", Order = 2 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CustomerId", Label = "Customer Id", Order = 3 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CustomerName", Label = "Customer", Order = 4 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductId", Label = "Product Id", Order = 5 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductName", Label = "Product", Order = 4 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductType", Label = "Product Type", Order = 5 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RateType", Label = "Rate Type", Order = 6 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RepaymentType", Label = "Repayment Type", Order = 7 });
            redressCasesConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Status", Label = "Status", Order = 8 });

            dbContext.SaveChanges();
        }

        private static void RedressConfig()
        {
            var redressConfig = new Config
            {
                Name = "Redress",
                Title = "Redress",
                Description = "Manage a RemediatR redress case",
                Model = "Headway.RemediatR.Core.Model.Redress, Headway.RemediatR.Core",
                ModelApi = "RemediatRRedress",
                CreateLocal = false,
                Document = "Headway.Razor.Controls.Documents.TabDocument`1, Headway.Razor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "RedressCases"
            };

            dbContext.Configs.Add(redressConfig);

            var redressConfigContainer = new ConfigContainer { Name = "Redress Div", Code = "REDRESS DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Redress Details", Order = 1 };
            var redressCustomerConfigContainer = new ConfigContainer { Name = "Redress Customer Tab Div", Code = "REDRESS CUSTOMER TAB DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Customer", Order = 2 };
            var redressProductConfigContainer = new ConfigContainer { Name = "Redress Product Tab Div", Code = "REDRESS PRODUCT TAB DIV", Container = "Headway.Razor.Controls.Containers.Div, Headway.Razor.Controls", Label = "Product", Order = 3 };

            redressConfig.ConfigContainers.Add(redressConfigContainer);
            redressConfig.ConfigContainers.Add(redressCustomerConfigContainer);
            redressConfig.ConfigContainers.Add(redressProductConfigContainer);

            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressId", Label = "Redress Id", IsIdentity = true, Order = 1, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Program", Label = "Program", Order = 2, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.GenericDropdown, Headway.Razor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={RemediatROptions.PROGRAMS_COMPLEX_OPTION_ITEMS}|Name={Options.DISPLAY_FIELD};Value=Name|Name={Args.MODEL};Value=Headway.RemediatR.Core.Model.Program, Headway.RemediatR.Core|Name={Args.COMPONENT};Value=Headway.Razor.Controls.Components.DropdownComplex`1, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CustomerName", Label = "Customer", IsTitle = true, Order = 3, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductName", Label = "Product", IsTitle = false, Order = 4, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
 
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressCaseOwner", Label = "Redress Case Owner", IsTitle = false, Order = 5, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressCreateBy", Label = "Redress Create By", IsTitle = false, Order = 6, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressCreateDate", Label = "Redress Create Date", IsTitle = false, Order = 7, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });
            
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RefundReviewStatus", Label = "Refund Review Status", IsTitle = false, Order = 8, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RefundReviewComment", Label = "Refund Review Comment", IsTitle = false, Order = 9, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.TextMultiline, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RefundReviewBy", Label = "Refund Review By", IsTitle = false, Order = 10, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RefundReviewDate", Label = "Refund Review Date", IsTitle = false, Order = 11, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });
            
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressReviewStatus", Label = "Redress Review Status", IsTitle = false, Order = 12, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressReviewComment", Label = "Redress Review Comment", IsTitle = false, Order = 13, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.TextMultiline, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressReviewBy", Label = "Redress Review By", IsTitle = false, Order = 14, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressReviewDate", Label = "Redress Review Date", IsTitle = false, Order = 15, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });

            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressValidationStatus", Label = "Redress Validation Status", IsTitle = false, Order = 16, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressValidationComment", Label = "Redress Validation Comment", IsTitle = false, Order = 17, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.TextMultiline, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressValidationBy", Label = "Redress Validation By", IsTitle = false, Order = 18, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RedressValidationDate", Label = "Redress Validation Date", IsTitle = false, Order = 19, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });

            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CommunicationGenerationStatus", Label = "Communication Generation Status", IsTitle = false, Order = 20, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CommunicationGenerationBy", Label = "Communication Generation By", IsTitle = false, Order = 21, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CommunicationGenerationDate", Label = "Communication Generation Date", IsTitle = false, Order = 22, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });

            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CommunicationDispatchStatus", Label = "Communication Dispatch Status", IsTitle = false, Order = 23, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CommunicationDispatchComment", Label = "Communication Dispatch Comment", IsTitle = false, Order = 24, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.TextMultiline, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CommunicationDispatchBy", Label = "Communication Dispatch By", IsTitle = false, Order = 25, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CommunicationDispatchDate", Label = "Communication Dispatch Date", IsTitle = false, Order = 26, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });

            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ResponseRequired", Label = "Response Required", IsTitle = false, Order = 27, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.CheckboxNullable, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ResponseReceived", Label = "Response Received", IsTitle = false, Order = 28, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.CheckboxNullable, Headway.Razor.Controls" });

            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AwaitingResponseStatus", Label = "Awaiting Response Status", IsTitle = false, Order = 29, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AwaitingResponseComment", Label = "Awaiting Response Comment", IsTitle = false, Order = 30, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.TextMultiline, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AwaitingResponseBy", Label = "Awaiting Response By", IsTitle = false, Order = 31, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AwaitingResponseDate", Label = "Awaiting Response Date", IsTitle = false, Order = 32, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });

            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PaymentGenerationStatus", Label = "Payment Generation Status", IsTitle = false, Order = 33, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PaymentGenerationBy", Label = "Payment Generation By", IsTitle = false, Order = 34, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PaymentGenerationDate", Label = "Payment Generation Date", IsTitle = false, Order = 35, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });

            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FinalRedressReviewStatus", Label = "Final Redress Review Status", IsTitle = false, Order = 36, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FinalRedressReviewComment", Label = "Final Redress Review Comment", IsTitle = false, Order = 37, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.TextMultiline, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FinalRedressReviewBy", Label = "Final Redress Review By", IsTitle = false, Order = 38, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.Label, Headway.Razor.Controls" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FinalRedressReviewDate", Label = "Final Redress Review Date", IsTitle = false, Order = 39, ConfigContainer = redressConfigContainer, Component = "Headway.Razor.Controls.Components.DateNullable, Headway.Razor.Controls" });

            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Customer", Label = "Customer", Order = 40, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "RedressCustomer" });
            redressConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Product", Label = "Product", Order = 41, ConfigContainer = redressProductConfigContainer, Component = "Headway.Razor.Controls.Components.GenericField, Headway.Razor.Controls", ConfigName = "RedressProduct" });

            dbContext.SaveChanges();
        }
    }
}
