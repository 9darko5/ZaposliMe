using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.ViewModels.City;
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
        public DbSet<UserApplicationView> UserApplicationViews { get; set; }
        public DbSet<EmployerApplicationView> EmployerApplicationViews { get; set; }
        public DbSet<CityView> CityViews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema(DefaultSchemaName);
        }
    }
}
