using Headway.Core.Constants;
using Headway.Core.Model;
using Microsoft.EntityFrameworkCore;
using RemediatR.Core.Constants;
using RemediatR.Core.Enums;
using RemediatR.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Headway.SeedData.RemediatR
{
    public class RemediatRData
    {
        public readonly static Dictionary<string, Permission> Permissions = new();
        public readonly static Dictionary<string, Role> Roles = new();
        public readonly static Dictionary<string, User> Users = new();

        public static List<Country> CountriesCreate()
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

            return countries;
        }

        public static void PermissionsCreate()
        {
            Permissions.Add(RemediatRAuthorisation.CUSTOMER_READ, new Permission { Name = RemediatRAuthorisation.CUSTOMER_READ, Description = "RemediatR Customer Read" });
            Permissions.Add(RemediatRAuthorisation.CUSTOMER_WRITE, new Permission { Name = RemediatRAuthorisation.CUSTOMER_WRITE, Description = "RemediatR Customer Write" });

            Permissions.Add(RemediatRAuthorisation.REDRESS_READ, new Permission { Name = RemediatRAuthorisation.REDRESS_READ, Description = "RemediatR Redress Read" });
            Permissions.Add(RemediatRAuthorisation.REDRESS_WRITE, new Permission { Name = RemediatRAuthorisation.REDRESS_WRITE, Description = "RemediatR Redress Write" });
            Permissions.Add(RemediatRAuthorisation.REDRESS_TRANSITION, new Permission { Name = RemediatRAuthorisation.REDRESS_TRANSITION, Description = "RemediatR Redress Transition" });
            Permissions.Add(RemediatRAuthorisation.COMMUNICATION_DISPATCH_TRANSITION, new Permission { Name = RemediatRAuthorisation.COMMUNICATION_DISPATCH_TRANSITION, Description = "RemediatR Communication Dispatch Transition" });
            Permissions.Add(RemediatRAuthorisation.AWAITING_REPONSE_TRANSITION, new Permission { Name = RemediatRAuthorisation.AWAITING_REPONSE_TRANSITION, Description = "RemediatR Awaiting Response Transition" });

            Permissions.Add(RemediatRAuthorisation.REDRESS_REVIEW_TRANSITION, new Permission { Name = RemediatRAuthorisation.REDRESS_REVIEW_TRANSITION, Description = "RemediatR Redress Refund Review Transition" });
            Permissions.Add(RemediatRAuthorisation.REDRESS_COMPLETE, new Permission { Name = RemediatRAuthorisation.REDRESS_COMPLETE, Description = "RemediatR Redress Complete" });

            Permissions.Add(RemediatRAuthorisation.REFUND_READ, new Permission { Name = RemediatRAuthorisation.REFUND_READ, Description = "RemediatR Refund Read" });
            Permissions.Add(RemediatRAuthorisation.REFUND_WRITE, new Permission { Name = RemediatRAuthorisation.REFUND_WRITE, Description = "RemediatR Refund Write" });
            Permissions.Add(RemediatRAuthorisation.REFUND_CACULATION_COMPLETE, new Permission { Name = RemediatRAuthorisation.REFUND_CACULATION_COMPLETE, Description = "RemediatR Refund Calculation Complete" });
            Permissions.Add(RemediatRAuthorisation.REFUND_VERIFICATION_COMPLETE, new Permission { Name = RemediatRAuthorisation.REFUND_VERIFICATION_COMPLETE, Description = "RemediatR Refund Varification Complete" });

            Permissions.Add(RemediatRAuthorisation.REFUND_REVIEW_TRANSITION, new Permission { Name = RemediatRAuthorisation.REFUND_REVIEW_TRANSITION, Description = "RemediatR Refund Review Transition" });
        }

        public static void RolesCreate()
        {
            Roles.Add(RemediatRAuthorisation.REDRESS_CASE_OWNER, new Role { Name = RemediatRAuthorisation.REDRESS_CASE_OWNER, Description = "RemediatR Redress Case Owner" });
            Roles.Add(RemediatRAuthorisation.REDRESS_REVIEWER, new Role { Name = RemediatRAuthorisation.REDRESS_REVIEWER, Description = "RemediatR Redress Reviewer" });
            Roles.Add(RemediatRAuthorisation.REFUND_ASSESSOR, new Role { Name = RemediatRAuthorisation.REFUND_ASSESSOR, Description = "RemediatR Refund Assessor" });
            Roles.Add(RemediatRAuthorisation.REFUND_REVIEWER, new Role { Name = RemediatRAuthorisation.REFUND_REVIEWER, Description = "RemediatR Refund Reviewer" });
        }

        public static void RolesAssignPermissions()
        {
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.CUSTOMER_READ]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.CUSTOMER_WRITE]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_READ]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_WRITE]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_TRANSITION]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_READ]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.COMMUNICATION_DISPATCH_TRANSITION]);
            Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER].Permissions.Add(Permissions[RemediatRAuthorisation.AWAITING_REPONSE_TRANSITION]);

            Roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.CUSTOMER_READ]);
            Roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_READ]);
            Roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_READ]);
            Roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_REVIEW_TRANSITION]);
            Roles[RemediatRAuthorisation.REDRESS_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_COMPLETE]);

            Roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_READ]);
            Roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_WRITE]);
            Roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_READ]);
            Roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_CACULATION_COMPLETE]);
            Roles[RemediatRAuthorisation.REFUND_ASSESSOR].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_VERIFICATION_COMPLETE]);

            Roles[RemediatRAuthorisation.REFUND_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_READ]);
            Roles[RemediatRAuthorisation.REFUND_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REDRESS_READ]);
            Roles[RemediatRAuthorisation.REFUND_REVIEWER].Permissions.Add(Permissions[RemediatRAuthorisation.REFUND_REVIEW_TRANSITION]);
        }

        public static void UsersCreate()
        {
            Users.Add("grace", new User { UserName = "grace", Email = "grace@email.com" });
            Users.Add("mel", new User { UserName = "mel", Email = "mel@email.com" });
            Users.Add("bill", new User { UserName = "bill", Email = "bill@email.com" });
            Users.Add("will", new User { UserName = "will", Email = "will@email.com" });
            Users.Add("mary", new User { UserName = "mary", Email = "mary@email.com" });
        }

        public static void UsersAssignRoles(bool assignAllUsersHeadwayUserRole = true)
        {
            if (assignAllUsersHeadwayUserRole)
            {
                foreach (var user in Users.Values)
                {
                    user.Roles.Add(Roles[HeadwayAuthorisation.USER]);
                }
            }

            Users["grace"].Roles.Add(Roles[RemediatRAuthorisation.REDRESS_CASE_OWNER]);
            Users["mel"].Roles.Add(Roles[RemediatRAuthorisation.REDRESS_REVIEWER]);
            Users["bill"].Roles.Add(Roles[RemediatRAuthorisation.REFUND_ASSESSOR]);
            Users["will"].Roles.Add(Roles[RemediatRAuthorisation.REFUND_ASSESSOR]);
            Users["mary"].Roles.Add(Roles[RemediatRAuthorisation.REFUND_REVIEWER]);
        }

        public static List<Program> ProgramsCreate()
        {
            List<Program> programs = new List<Program>
            {
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
                },
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
                },
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
                }
            };

            return programs;
        }

        public static List<Customer> CustomersCreate()
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
                        StartDate = DateTime.ParseExact(p[7], "yyyy-MM-ddTHH:mm:ss.fffz", null),
                        Value = decimal.Parse(p[8])
                    });
                }

                customers.Add(customer);
            }

            return customers;
        }

        public static Config ProgramsConfigCreate()
        {
            var programsConfig = new Config
            {
                Name = "Programs",
                Title = "Programs",
                Description = "List of RemediatR programs",
                Model = "RemediatR.Core.Model.Program, RemediatR.Core",
                ModelApi = "RemediatRProgram",
                OrderModelBy = "Name",
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Css.HEADER_BTN_IMAGE};Value={Css.BTN_IMAGE_PLUS}|Name={Css.HEADER_BTN_TOOLTIP};Value={Css.BTN_TOOLTIP_CREATE}|Name={Css.ROW_BTN_IMAGE};Value={Css.BTN_IMAGE_EDIT}|Name={Css.ROW_BTN_TOOLTIP};Value={Css.BTN_TOOLTIP_EDIT}",
                NavigatePage = "Page",
                NavigateProperty = "ProgramId",
                NavigateConfig = "Program",
                NavigateResetBreadcrumb = true
            };

            programsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProgramId", Label = "Program Id", Order = 1 });
            programsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", Order = 2 });
            programsConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3 });

            return programsConfig;
        }

        public static Config ProgramConfigCreate()
        {
            var programConfig = new Config
            {
                Name = "Program",
                Title = "Program",
                Description = "Create, update or delete a RemediatR program",
                Model = "RemediatR.Core.Model.Program, RemediatR.Core",
                ModelApi = "RemediatRProgram",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.TabDocument`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Programs"
            };

            var programConfigContainer = new ConfigContainer { Name = "Program Div", Code = "PROGRAM DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Program", Order = 1 };

            programConfig.ConfigContainers.Add(programConfigContainer);

            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProgramId", Label = "Program Id", IsIdentity = true, Order = 1, ConfigContainer = programConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = programConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Description", Label = "Description", Order = 3, ConfigContainer = programConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Compensation", Label = "Compensation", Order = 4, ConfigContainer = programConfigContainer, Component = "Headway.Blazor.Controls.Components.DecimalNullable, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.FORMAT};Value={Args.FORMAT_F2}|Name={Args.MAX};Value=9999999999999.99" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CompensatoryInterest", Label = "Compensatory Interest", Order = 5, ConfigContainer = programConfigContainer, Component = "Headway.Blazor.Controls.Components.DecimalNullable, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.FORMAT};Value={Args.FORMAT_F2}|Name={Args.MAX_LENGTH};Value=5|Name={Args.MAX};Value=99.99" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StartDate", Label = "Start Date", Order = 6, ConfigContainer = programConfigContainer, Component = "Headway.Blazor.Controls.Components.DateNullable, Headway.Blazor.Controls" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "EndDate", Label = "End Date", Order = 7, ConfigContainer = programConfigContainer, Component = "Headway.Blazor.Controls.Components.DateNullable, Headway.Blazor.Controls" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductType", Label = "ProductType", Order = 8, ConfigContainer = programConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownEnum`1, Headway.Blazor.Controls|Name={Args.MODEL};Value=RemediatR.Core.Enums.ProductType, RemediatR.Core" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RateType", Label = "RateType", Order = 9, ConfigContainer = programConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownEnum`1, Headway.Blazor.Controls|Name={Args.MODEL};Value=RemediatR.Core.Enums.RateType, RemediatR.Core" });
            programConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RepaymentType", Label = "RepaymentType", Order = 10, ConfigContainer = programConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownEnum`1, Headway.Blazor.Controls|Name={Args.MODEL};Value=RemediatR.Core.Enums.RepaymentType, RemediatR.Core" });

            return programConfig;
        }

        public static Config CustomersConfigCreate()
        {
            var customersConfig = new Config
            {
                Name = "Customers",
                Title = "Customers",
                Description = "List of RemediatR customers",
                Model = "RemediatR.Core.Model.Customer, RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                OrderModelBy = "Surname",
                Document = "Headway.Blazor.Controls.Documents.Table`1, Headway.Blazor.Controls",
                DocumentArgs = $"Name={Css.HEADER_BTN_IMAGE};Value={Css.BTN_IMAGE_PLUS}|Name={Css.HEADER_BTN_TOOLTIP};Value={Css.BTN_TOOLTIP_CREATE}|Name={Css.ROW_BTN_IMAGE};Value={Css.BTN_IMAGE_EDIT}|Name={Css.ROW_BTN_TOOLTIP};Value={Css.BTN_TOOLTIP_EDIT}",
                SearchComponent = "Headway.Blazor.Controls.SearchComponents.StandardSearchComponent, Headway.Blazor.Controls",
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
                    Component = "Headway.Blazor.Controls.SearchComponents.SearchText, Headway.Blazor.Controls",
                    Order = 1
                },
                new ConfigSearchItem
                {
                    Label = "Surname",
                    ParameterName = "Surname",
                    Tooltip = "The customer surname",
                    Component = "Headway.Blazor.Controls.SearchComponents.SearchText, Headway.Blazor.Controls",
                    Order = 2
                }
            });

            customersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CustomerId", Label = "Customer Id", Order = 1 });
            customersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", Order = 2 });
            customersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FirstName", Label = "First Name", Order = 3 });
            customersConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Surname", Label = "Surname", Order = 4 });

            return customersConfig;
        }

        public static Config CustomerConfigCreate()
        {
            var customerConfig = new Config
            {
                Name = "Customer",
                Title = "Customer",
                Description = "Create, update or delete a RemediatR customer",
                Model = "RemediatR.Core.Model.Customer, RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.TabDocument`1, Headway.Blazor.Controls",
                NavigatePage = "Page",
                NavigateConfig = "Customers"
            };

            var customerConfigContainer = new ConfigContainer { Name = "Customer Div", Code = "CUSTOMER DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Customer", Order = 1 };
            var productsConfigContainer = new ConfigContainer { Name = "Products Div", Code = "PRODUCTS DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Products", Order = 2 };

            customerConfig.ConfigContainers.Add(customerConfigContainer);
            customerConfig.ConfigContainers.Add(productsConfigContainer);

            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CustomerId", Label = "Customer Id", IsIdentity = true, Order = 1, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", Order = 2, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.STATIC_OPTION_ITEMS}|Name=Ms;Value=Ms|Name=Mr;Value=Mr|Name=Mrs;Value=Mrs" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FirstName", Label = "FirstName", Order = 3, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Surname", Label = "Surname", IsTitle = true, Order = 4, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AccountStatus", Label = "Account Status", Order = 5, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownEnum`1, Headway.Blazor.Controls|Name={Args.MODEL};Value=RemediatR.Core.Enums.AccountStatus, RemediatR.Core" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Telephone", Label = "Telephone", Order = 6, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Email", Label = "Email", Order = 7, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "SortCode", Label = "Sort Code", Order = 8, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AccountNumber", Label = "Account Number", Order = 9, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address1", Label = "Address1", Order = 10, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address2", Label = "Address2", Order = 11, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address3", Label = "Address3", Order = 12, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address4", Label = "Address4", Order = 13, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address5", Label = "Address5", Order = 14, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Country", Label = "Country", Order = 15, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.COUNTRY_OPTION_ITEMS}" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PostCode", Label = "PostCode", Order = 16, ConfigContainer = customerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            customerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Products", Label = "Products", Order = 17, ConfigContainer = productsConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericField, Headway.Blazor.Controls", ConfigName = "Product", ComponentArgs = $"Name={Args.LIST_CONFIG};Value=ProductsListDetail" });

            return customerConfig;
        }

        public static Config ProductConfigCreate()
        {
            var productConfig = new Config
            {
                Name = "Product",
                Title = "Product",
                Description = "Product sold to the customer",
                Model = "RemediatR.Core.Model.Product, RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                CreateLocal = true,
                Document = "Headway.Blazor.Controls.Documents.ListDetail`1, Headway.Blazor.Controls"
            };

            var productConfigContainer = new ConfigContainer { Name = "Product Div", Code = "PRODUCT DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Product", Order = 1 };

            productConfig.ConfigContainers.Add(productConfigContainer);

            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductId", Label = "Product Id", IsIdentity = true, Order = 1, ConfigContainer = productConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Name", IsTitle = true, Order = 2, ConfigContainer = productConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Value", Label = "Value", Order = 3, ConfigContainer = productConfigContainer, Component = "Headway.Blazor.Controls.Components.DecimalNullable, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.FORMAT};Value={Args.FORMAT_F2}|Name={Args.MAX};Value=9999999999999.99" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Rate", Label = "Rate", Order = 4, ConfigContainer = productConfigContainer, Component = "Headway.Blazor.Controls.Components.DecimalNullable, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.FORMAT};Value={Args.FORMAT_F2}|Name={Args.MAX_LENGTH};Value=5|Name={Args.MAX};Value=99.99" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Duration", Label = "Duration", Order = 5, ConfigContainer = productConfigContainer, Component = "Headway.Blazor.Controls.Components.IntegerNullable, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.MIN};Value=3|Name={Args.MAX};Value=360", Tooltip = "Duration must be min 3 and max 360" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StartDate", Label = "Start Date", Order = 6, ConfigContainer = productConfigContainer, Component = "Headway.Blazor.Controls.Components.DateNullable, Headway.Blazor.Controls" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "ProductType", Label = "ProductType", Order = 7, ConfigContainer = productConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownEnum`1, Headway.Blazor.Controls|Name={Args.MODEL};Value=RemediatR.Core.Enums.ProductType, RemediatR.Core" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RateType", Label = "RateType", Order = 8, ConfigContainer = productConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownEnum`1, Headway.Blazor.Controls|Name={Args.MODEL};Value=RemediatR.Core.Enums.RateType, RemediatR.Core" });
            productConfig.ConfigItems.Add(new ConfigItem { PropertyName = "RepaymentType", Label = "RepaymentType", Order = 9, ConfigContainer = productConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownEnum`1, Headway.Blazor.Controls|Name={Args.MODEL};Value=RemediatR.Core.Enums.RepaymentType, RemediatR.Core" });

            return productConfig;
        }

        public static Config ProductsListDetailConfigCreate()
        {
            var productsListDetailConfig = new Config
            {
                Name = "ProductsListDetail",
                Title = "ProductsListDetail",
                Description = "List of products sold to the customer",
                Model = "RemediatR.Core.Model.Product, RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                OrderModelBy = "StartDate"
            };

            productsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Name", Label = "Product Name", Order = 1 });
            productsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "StartDate", Label = "Start Date", Order = 2 });
            productsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Duration", Label = "Duration", Order = 3 });
            productsListDetailConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Value", Label = "Value", Order = 4 });

            return productsListDetailConfig;
        }

        public static Config RedressCustomerConfigCreate()
        {
            var redressCustomerConfig = new Config
            {
                Name = "RedressCustomer",
                Title = "Customer",
                Description = "RemediatR customer",
                Model = "RemediatR.Core.Model.Customer, RemediatR.Core",
                ModelApi = "RemediatRCustomer",
                Document = "Headway.Blazor.Controls.Documents.Content`1, Headway.Blazor.Controls"
            };

            var redressCustomerConfigContainer = new ConfigContainer { Name = "Redress Customer Div", Code = "REDRESS CUSTOMER DIV", Container = "Headway.Blazor.Controls.Containers.Div, Headway.Blazor.Controls", Label = "Customer", Order = 1 };

            redressCustomerConfig.ConfigContainers.Add(redressCustomerConfigContainer);

            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "CustomerId", Label = "Customer Id", IsIdentity = true, Order = 1, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Label, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Title", Label = "Title", Order = 2, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.STATIC_OPTION_ITEMS}|Name=Ms;Value=Ms|Name=Mr;Value=Mr|Name=Mrs;Value=Mrs" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "FirstName", Label = "FirstName", Order = 3, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Surname", Label = "Surname", IsTitle = true, Order = 4, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AccountStatus", Label = "Account Status", Order = 5, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.GenericDropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Args.COMPONENT};Value=Headway.Blazor.Controls.Components.DropdownEnum`1, Headway.Blazor.Controls|Name={Args.MODEL};Value=RemediatR.Core.Enums.AccountStatus, RemediatR.Core" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Telephone", Label = "Telephone", Order = 6, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Email", Label = "Email", Order = 7, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "SortCode", Label = "Sort Code", Order = 8, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "AccountNumber", Label = "Account Number", Order = 9, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address1", Label = "Address1", Order = 10, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address2", Label = "Address2", Order = 11, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address3", Label = "Address3", Order = 12, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address4", Label = "Address4", Order = 13, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Address5", Label = "Address5", Order = 14, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "Country", Label = "Country", Order = 15, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Dropdown, Headway.Blazor.Controls", ComponentArgs = $"Name={Options.OPTIONS_CODE};Value={Options.COUNTRY_OPTION_ITEMS}" });
            redressCustomerConfig.ConfigItems.Add(new ConfigItem { PropertyName = "PostCode", Label = "PostCode", Order = 16, ConfigContainer = redressCustomerConfigContainer, Component = "Headway.Blazor.Controls.Components.Text, Headway.Blazor.Controls" });

            return redressCustomerConfig;
        }
    }
}