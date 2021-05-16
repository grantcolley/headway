using Headway.Database.Data;
using Headway.Database.OptionsBuilder;
using Microsoft.EntityFrameworkCore;
using System;
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

                var optionsBuilder = optionsBuilderFactory.GetOptionsBuilder<HeadwayDbContext>(databaseType);
                using var dbContext = new HeadwayDbContext(optionsBuilder.GetOptions());
                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();
                Console.WriteLine($"Created Data Source: {optionsBuilder.DataSource}");
            }
        }
    }
}
