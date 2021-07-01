using Headway.Core.Model;
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
        public DbSet<ModelConfig> ModelConfigs { get; set; }
        public DbSet<FieldConfig> FieldConfigs { get; set; }
        public DbSet<ListConfig> ListConfigs { get; set; }
        public DbSet<ListItemConfig> ListItemConfigs { get; set; }

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
                .HasIndex(p => p.Name)
                .IsUnique();

            builder.Entity<MenuItem>()
                .HasIndex(p => p.Name)
                .IsUnique();

            builder.Entity<ModelConfig>()
                .HasIndex(p => p.ModelName)
                .IsUnique();

            builder.Entity<ListConfig>()
                .HasIndex(p => p.ListName)
                .IsUnique();
        }
    }
}