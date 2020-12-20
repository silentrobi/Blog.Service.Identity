using Blog.Service.Identity.Domain.Role;
using Blog.Service.Identity.Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Blog.Service.Identity.Infrastructure.Contexts
{
    public class ApplicationIdentityDbContext : IdentityDbContext<User,Role, Guid>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasPostgresExtension("uuid-ossp");

            builder.Entity<User>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("uuid_generate_v4()");
            });

            builder.Entity<Role>(b =>
            {
                b.Property(u => u.Id).HasDefaultValueSql("uuid_generate_v4()");
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
