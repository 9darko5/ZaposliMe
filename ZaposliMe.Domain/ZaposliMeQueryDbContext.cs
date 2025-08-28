using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.ViewModels.Job;

namespace ZaposliMe.Persistance
{
    public class ZaposliMeQueryDbContext : DbContext
    {
        private const string DefaultSchemaName = "zaposlime";
        public ZaposliMeQueryDbContext(DbContextOptions<ZaposliMeQueryDbContext> options)
            : base(options)
        {
        }

        public DbSet<JobGridView> JobGridViews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema(DefaultSchemaName);
        }
    }
}
