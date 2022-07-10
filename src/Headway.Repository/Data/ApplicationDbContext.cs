using Headway.Core.Model;
using Headway.RemediatR.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Headway.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<ConfigItem> ConfigItems { get; set; }
        public DbSet<ConfigContainer> ConfigContainers { get; set; }
        public DbSet<DemoModel> DemoModels { get; set; }
        public DbSet<DemoModelItem> DemoModelItems { get; set; }
        public DbSet<DemoModelTreeItem> DemoModelTreeItems { get; set; }
        public DbSet<DemoModelComplexProperty> DemoModelComplexProperties { get; set; }

        // RemediatR
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Redress> Redresses { get; set; }
        public DbSet<RedressProduct> RedressProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            builder.Entity<Permission>()
                .HasIndex(p => p.Name)
                .IsUnique();

            builder.Entity<Module>()
                .HasIndex(p => p.Name)
                .IsUnique();

            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<MenuItem>()
                .HasIndex(m => m.Name)
                .IsUnique();

            builder.Entity<Config>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<ConfigContainer>()
                .HasIndex(cc => cc.Name)
                .IsUnique();

            builder.Entity<Country>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<Program>()
                .HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}