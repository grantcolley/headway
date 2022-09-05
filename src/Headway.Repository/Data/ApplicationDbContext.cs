using Headway.Core.Model;
using Headway.RemediatR.Core.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;
using Headway.Repository.Model;
using System.Collections.Generic;
using System.Collections;
using System.Text.Json;

namespace Headway.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        private string user;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EntityAudit> EntityAudits { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<ConfigItem> ConfigItems { get; set; }
        public DbSet<ConfigContainer> ConfigContainers { get; set; }
        public DbSet<ConfigSearchItem> ConfigSearchItems { get; set; }
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

        public void SetUser(string user)
        {
            this.user = user;
        }

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

        public override int SaveChanges()
        {
            UpdateModelBaseFields();

            var entityAudits = OnBeforeSaveChanges();

            var result = base.SaveChanges();

            if (entityAudits.Any())
            {
                OnAfterSaveChanges(entityAudits);

                base.SaveChanges();
            }

            return result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateModelBaseFields();

            var entityAudits = OnBeforeSaveChanges();

            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            if (entityAudits.Any())
            {
                OnAfterSaveChanges(entityAudits);

                base.SaveChanges(acceptAllChangesOnSuccess);
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateModelBaseFields();

            var entityAudits = OnBeforeSaveChanges();

            var result = await base.SaveChangesAsync(cancellationToken);

            if (entityAudits.Any())
            {
                OnAfterSaveChanges(entityAudits);

                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateModelBaseFields();

            var entityAudits = OnBeforeSaveChanges();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            if(entityAudits.Any())
            {
                OnAfterSaveChanges(entityAudits);

                await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }

            return result;
        }

        public void UpdateModelBaseFields()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is ModelBase
                        && (e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var now = DateTime.Now;

                ((ModelBase)entityEntry.Entity).ModifiedDate = now;
                ((ModelBase)entityEntry.Entity).ModifiedBy = user ?? null;

                if (entityEntry.State == EntityState.Added)
                {
                    ((ModelBase)entityEntry.Entity).CreatedDate = now;
                    ((ModelBase)entityEntry.Entity).CreatedBy = user ?? null;
                }
            }
        }

        private List<EntityAudit> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();

            var entityAudits = new List<EntityAudit>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is not ModelBase
                    || entry.State == EntityState.Detached
                    || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var entityAudit = new EntityAudit
                {
                    TableName = entry.Metadata.GetTableName(),
                    ClrType = entry.Metadata.ClrType.Name,
                    Action = entry.State == EntityState.Added ? "ADD" : entry.State == EntityState.Modified ? "UPDATE" : "DELETE"
                };

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        entityAudit.TemporaryProperties.Add(property);
                    }

                    if (property.Metadata.IsPrimaryKey())
                    {
                        entityAudit.EntityId = property.CurrentValue.ToString();
                    }

                    string propertyName = property.Metadata.Name;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entityAudit.NewValuesDictionary[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            entityAudit.OldValuesDictionary[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                entityAudit.OldValuesDictionary[propertyName] = property.OriginalValue;
                                entityAudit.NewValuesDictionary[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }

                entityAudits.Add(entityAudit);
            }

            foreach(var entityAudit in entityAudits.Where(e => !e.TemporaryProperties.Any()))
            {
                EntityAudits.Add(SerializeValues(entityAudit));
            }

            return entityAudits.Where(e => e.TemporaryProperties.Any()).ToList();
        }

        private void OnAfterSaveChanges(List<EntityAudit> entityAudits)
        {
            foreach (var entityAudit in entityAudits)
            {
                foreach (var prop in entityAudit.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        entityAudit.EntityId = prop.CurrentValue.ToString();
                    }

                    entityAudit.NewValuesDictionary[prop.Metadata.Name] = prop.CurrentValue;
                }

                EntityAudits.Add(SerializeValues(entityAudit));
            }
        }

        private static EntityAudit SerializeValues(EntityAudit entityAudit)
        {
            if(entityAudit.OldValuesDictionary.Any())
            {
                entityAudit.OldValues = JsonSerializer.Serialize(entityAudit.OldValuesDictionary);
            }

            if (entityAudit.NewValuesDictionary.Any())
            {
                entityAudit.NewValues = JsonSerializer.Serialize(entityAudit.NewValuesDictionary);
            }

            return entityAudit;
        }
    }
}