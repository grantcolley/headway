using Headway.Database.Data;
using Headway.Database.OptionsBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;

namespace Headway.Database
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataProviders = new string[] { "sqlite", "sqlserver" };
            if(args == null
                || args.Length != 1
                || !dataProviders.Contains(args[0]))
            {
                Console.WriteLine($"args must be one of the following supported data providers: {string.Join(",", dataProviders)}");
            }
            else
            {
                string databaseType = args[0];

                var optionsBuilderFactory = new HeadwayDbContextOptionsBuilderFactory();
                var optionsBuilder = optionsBuilderFactory.GetOptionsBuilder<ApplicationDbContext>(databaseType);

                using var dbContext = new ApplicationDbContext(optionsBuilder.GetOptions());

                // https://github.com/dotnet/efcore/issues/23595
                // https://docs.microsoft.com/en-us/ef/core/cli/services#using-services

                /////////////////////////////////////////////////////
                /// OLD
                /// https://github.com/dotnet/efcore/issues/6806
                /////////////////////////////////////////////////////

                //// Create design-time services
                //var serviceCollection = new ServiceCollection();
                //serviceCollection.AddEntityFrameworkDesignTimeServices();
                //serviceCollection.AddDbContextDesignTimeServices(dbContext);
                //var serviceProvider = serviceCollection.BuildServiceProvider();

                //// Add a migration
                //var migrationsScaffolder = serviceProvider.GetService<IMigrationsScaffolder>();
                //var migration = migrationsScaffolder.ScaffoldMigration("Headway", "Headway.Database");
                //migrationsScaffolder.Save(Directory.GetCurrentDirectory(), migration, Path.Combine("..\\", Directory.GetCurrentDirectory()));

                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();
                Console.WriteLine($"Created Data Source: {optionsBuilder.DataSource}");
            }
        }
    }
}
