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
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if(connectionString.Contains("Headway.db"))
            {
                builder.UseSqlite(connectionString, x => x.MigrationsAssembly("Headway.MigrationsSqlite"));
            }
            else
            {
                builder.UseSqlServer(connectionString, x => x.MigrationsAssembly("Headway.MigrationsSqlServer"));
            }

            return new ApplicationDbContext(builder.Options);
        }
    }
}
