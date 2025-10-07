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

            builder.Entity<Job>()
                .HasMany(j => j.Applications)
                .WithOne(a => a.Job)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
