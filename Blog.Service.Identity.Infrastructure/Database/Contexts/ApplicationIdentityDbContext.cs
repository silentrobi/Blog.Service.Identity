using Blog.Service.Identity.Domain.Role;
using Blog.Service.Identity.Domain.SeedWork;
using Blog.Service.Identity.Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Service.Identity.Infrastructure.Contexts
{
    public class ApplicationIdentityDbContext : IdentityDbContext<User,Role, Guid>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public  override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is IBaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow; // current datetime

                if (entity.State == EntityState.Added)
                {
                    ((IBaseEntity)entity.Entity).CreatedAt= now;
                }
                ((IBaseEntity)entity.Entity).UpdatedAt = now;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasPostgresExtension("uuid-ossp");

            builder.Entity<User>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("uuid_generate_v4()");
                b.Property(u => u.RecordStatus).HasDefaultValue(true);
            });

            builder.Entity<Role>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("uuid_generate_v4()");
                b.Property(u => u.RecordStatus).HasDefaultValue(true);
            });

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                const string tableNamePrefix = "AspNet";
                var tableName = entityType.GetTableName();

                if (tableName.StartsWith(tableNamePrefix))
                    entityType.SetTableName(tableName[tableNamePrefix.Length..]);
            }
        }
    }
}
