using Microsoft.EntityFrameworkCore;

namespace ZaposliMe.Domain
{
    public class ZaposliMeDbContext : DbContext
    {
        private const string DefaultSchemaName = "zaposlime";
        public ZaposliMeDbContext(DbContextOptions<ZaposliMeDbContext> options)
            : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasDefaultSchema(DefaultSchemaName);
        }
    }
}
