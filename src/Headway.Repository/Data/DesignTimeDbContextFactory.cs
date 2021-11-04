using Headway.Core.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Headway.Repository.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration
                = new ConfigurationBuilder().SetBasePath(
                    Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../Headway.WebApi/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();            
            var connectionString = configuration.GetConnectionString(DataMigrations.CONNECTION_STRING);
            if(connectionString.Contains(DataMigrations.SQLITE_DATABASE))
            {
                builder.UseSqlite(connectionString, x => x.MigrationsAssembly(DataMigrations.SQLITE_MIGRATIONS));
            }
            else
            {
                builder.UseSqlServer(connectionString, x => x.MigrationsAssembly(DataMigrations.SQLSERVER_MIGRATIONS));
            }

            return new ApplicationDbContext(builder.Options);
        }
    }
}
