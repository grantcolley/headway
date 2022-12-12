using Headway.Blazor.Controls.Flow;
using Headway.Core.Constants;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using RemediatR.Core.Constants;
using RemediatR.Core.Enums;
using RemediatR.Core.Model;
using RemediatR.Repository.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Headway.SeedData.RemediatR
{
    public class RemediatRSeedData
    {
        private static ApplicationDbContext dbContext;

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
            RefundCalculation();
            RefundVerification();
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
            var countries = RemediatRData.CountriesCreate();

            foreach (var country in countries)
            {
                dbContext.Countries.Add(country);
            }
            
            dbContext.SaveChanges();
        }

        private static void CreatePermissions()
        {
            RemediatRData.PermissionsCreate();

            foreach (var permission in RemediatRData.Permissions.Values)
            {
                dbContext.Permissions.Add(permission);
            }

            dbContext.SaveChanges();
        }

        private static void CreateRoles()
        {
            RemediatRData.RolesCreate();

            foreach (var role in RemediatRData.Roles.Values)
            {
                dbContext.Roles.Add(role);
            }

            RemediatRData.RolesAssignPermissions();

            dbContext.SaveChanges();
        }

        private static void CreateUsers()
        {
            RemediatRData.UsersCreate();

            foreach (var user in RemediatRData.Users.Values)
            {
                dbContext.Users.Add(user);
            }

            dbContext.SaveChanges();
        }

        private static void AssignUsersRoles()
        {
            var userRole = dbContext.Roles
                .FirstOrDefault(r => r.Name.Equals(HeadwayAuthorisation.USER));

            foreach(var user in RemediatRData.Users.Values)
            {
                user.Roles.Add(userRole);
            }

            RemediatRData.UsersAssignRoles(assignAllUsersHeadwayUserRole: false);

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
            var programs = RemediatRData.ProgramsCreate();

            foreach (var program in programs)
            {
                dbContext.Programs.Add(program);
            }

            dbContext.SaveChanges();
        }

        private static void CreateCustomers()
        {
            var customers = RemediatRData.CustomersCreate();

            foreach (var customer in customers)
            {
                dbContext.Customers.Add(customer);
            }

            dbContext.SaveChanges();
        }

        private static void ProgramsConfig()
        {
            var programsConfig = RemediatRData.ProgramsConfigCreate();

            dbContext.Configs.Add(programsConfig);

            dbContext.SaveChanges();
        }

        private static void ProgramConfig()
        {
            var programConfig = RemediatRData.ProgramConfigCreate();

            dbContext.Configs.Add(programConfig);

            dbContext.SaveChanges();
        }

        private static void CustomersConfig()
        {
            var customersConfig = RemediatRData.CustomersConfigCreate();

            dbContext.Configs.Add(customersConfig);

            dbContext.SaveChanges();
        }

        private static void CustomerConfig()
        {
            var customerConfig = RemediatRData.CustomerConfigCreate();

            dbContext.Configs.Add(customerConfig);

            dbContext.SaveChanges();
        }

        private static void ProductConfig()
        {
            var productConfig = RemediatRData.ProductConfigCreate();

            dbContext.Configs.Add(productConfig);

            dbContext.SaveChanges();
        }

        private static void ProductsListDetailConfig()
        {
            var productsListDetailConfig = RemediatRData.ProductsListDetailConfigCreate();

            dbContext.Configs.Add(productsListDetailConfig);

            dbContext.SaveChanges();
        }

        private static void RedressCustomerConfig()
        {
            var redressCustomerConfig = RemediatRData.RedressCustomerConfigCreate();

            dbContext.Configs.Add(redressCustomerConfig);

            dbContext.SaveChanges();
        }

        private static void RedressProductConfig()
        {
            var redressProductConfig = RemediatRData.RedressProductConfigCreate();

            dbContext.Configs.Add(redressProductConfig);

            dbContext.SaveChanges();
        }

        private static void RedressCasesConfig()
        {
            var redressCasesConfig = RemediatRData.RedressCasesConfigCreate();

            dbContext.Configs.Add(redressCasesConfig);

            dbContext.SaveChanges();
        }

        private static void NewRedressCasesConfig()
        {
            var redressCasesConfig = RemediatRData.NewRedressCasesConfigCreate();

            dbContext.Configs.Add(redressCasesConfig);

            dbContext.SaveChanges();
        }

        private static void RedressConfig()
        {
            var redressConfig = RemediatRData.RedressConfigCreate();

            dbContext.Configs.Add(redressConfig);
            
            dbContext.SaveChanges();
        }

        private static void RefundCalculation()
        {
            var refundCalculationConfig = RemediatRData.RefundCalculationCreate();

            dbContext.Configs.Add(refundCalculationConfig);

            dbContext.SaveChanges();
        }

        private static void RefundVerification()
        {
            var refundVerificationConfig = RemediatRData.RefundVerificationCreate();

            dbContext.Configs.Add(refundVerificationConfig);

            dbContext.SaveChanges();
        }
    }
}
