using Microsoft.EntityFrameworkCore;
using ZaposliMe.Persistance;

namespace ZaposliMe.WebAPI.Extensions
{
    public static class MigrateExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using UserManagementDbContext userManagementContext = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>();
            using ZaposliMeDbContext zaposliMeContext = scope.ServiceProvider.GetRequiredService<ZaposliMeDbContext>();

            userManagementContext.Database.Migrate();
            zaposliMeContext.Database.Migrate();
        }
    }
}
