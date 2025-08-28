using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.Entities;

namespace ZaposliMe.Persistance
{
    public class ZaposliMeDbContext : DbContext
    {
        private const string DefaultSchemaName = "zaposlime";
        public ZaposliMeDbContext(DbContextOptions<ZaposliMeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema(DefaultSchemaName);
        }
    }
}
