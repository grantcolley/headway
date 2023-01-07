using Headway.Core.Model;
using Headway.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RemediatR.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Headway.Repository.Data
{
    public class ApplicationDbContext : DbContext
    {
        private string user;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Audit> Audits { get; set; }

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

        // Flow
        public DbSet<Flow> Flows { get; set; }
        public DbSet<FlowHistory> FlowHistory { get; set; }
        public DbSet<State> States { get; set; }

        // RemediatR
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Redress> Redresses { get; set; }
        public DbSet<RefundCalculation> RefundCalculations { get; set; }

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

            builder.Entity<Flow>()
                .HasIndex(f => f.Name)
                .IsUnique();

            builder.Entity<State>()
                .HasIndex(s => s.StateCode)
                .IsUnique();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var audits = OnBeforeSaveChanges();

            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            if (audits.Any())
            {
                OnAfterSaveChanges(audits);

                base.SaveChanges(acceptAllChangesOnSuccess);
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var audits = OnBeforeSaveChanges();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            if(audits.Any())
            {
                OnAfterSaveChanges(audits);

                await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }

            return result;
        }

        private List<Audit> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();

            var audits = new List<Audit>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is not ModelBase
                    || entry.State == EntityState.Detached
                    || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var now = DateTime.Now;

                if (entry.State.Equals(EntityState.Added))
                {
                    ((ModelBase)entry.Entity).CreatedDate = now;
                    ((ModelBase)entry.Entity).CreatedBy = user ?? null;
                    ((ModelBase)entry.Entity).ModifiedDate = now;
                    ((ModelBase)entry.Entity).ModifiedBy = user ?? null;
                }
                else if (entry.State.Equals(EntityState.Modified))
                {
                    ((ModelBase)entry.Entity).ModifiedDate = now;
                    ((ModelBase)entry.Entity).ModifiedBy = user ?? null;
                }

                var audit = new Audit
                {
                    TableName = entry.Metadata.GetTableName(),
                    ClrType = entry.Metadata.ClrType.Name,
                    Action = entry.State == EntityState.Added ? "ADD" : entry.State == EntityState.Modified ? "UPDATE" : "DELETE",
                    User = user,
                    DateTime = now
                };

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        audit.TemporaryProperties.Add(property);
                    }

                    if (property.Metadata.IsPrimaryKey())
                    {
                        audit.EntityId = property.CurrentValue.ToString();
                    }

                    string propertyName = property.Metadata.Name;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            audit.NewValuesDictionary[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            audit.OldValuesDictionary[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                audit.OldValuesDictionary[propertyName] = property.OriginalValue;
                                audit.NewValuesDictionary[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }

                audits.Add(audit);
            }

            foreach(var audit in audits.Where(e => !e.TemporaryProperties.Any()))
            {
                Audits.Add(SerializeValues(audit));
            }

            return audits.Where(e => e.TemporaryProperties.Any()).ToList();
        }

        private void OnAfterSaveChanges(List<Audit> audits)
        {
            foreach (var audit in audits)
            {
                foreach (var prop in audit.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        audit.EntityId = prop.CurrentValue.ToString();
                    }

                    audit.NewValuesDictionary[prop.Metadata.Name] = prop.CurrentValue;
                }

                Audits.Add(SerializeValues(audit));
            }
        }

        private static Audit SerializeValues(Audit audit)
        {
            if(audit.OldValuesDictionary.Any())
            {
                audit.OldValues = JsonSerializer.Serialize(audit.OldValuesDictionary);
            }

            if (audit.NewValuesDictionary.Any())
            {
                audit.NewValues = JsonSerializer.Serialize(audit.NewValuesDictionary);
            }

            return audit;
        }
    }
}