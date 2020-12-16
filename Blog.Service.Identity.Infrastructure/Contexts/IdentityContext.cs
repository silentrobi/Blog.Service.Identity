using Blog.Service.Identity.Domain.IdentityUser;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.Identity.Infrastructure.Contexts
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<IdentityUser> IdentityUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
