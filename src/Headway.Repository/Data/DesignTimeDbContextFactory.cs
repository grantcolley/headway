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
                    Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../../Headway.WebApi/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString, x => x.MigrationsAssembly("Headway.DatabaseMigrations"));
            return new ApplicationDbContext(builder.Options);
        }
    }
}
