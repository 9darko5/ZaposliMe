using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.Entities.Identity;

namespace ZaposliMe.Persistance
{
    public class UserManagementDbContext : IdentityDbContext<User>
    {
        private const string DefaultSchemaName = "identity";
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(u => u.Initials).HasMaxLength(5);

            builder.HasDefaultSchema(DefaultSchemaName);
        }
    }
}
