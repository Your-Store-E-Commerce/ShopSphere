using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopSphere.Data.Context;

namespace ShopSphere.Web.Helper
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync (WebApplication app)
        {
            using var Scope = app.Services.CreateScope();

            var Service = Scope.ServiceProvider;

            var _context = Service.GetRequiredService<ShopSphere_DbContext>();




            var loggerfactory = Service.GetRequiredService<ILoggerFactory>();

            try
            {
                await _context.Database.MigrateAsync();

            }
            catch (Exception ex)
            {
                var logger = loggerfactory.CreateLogger<Program>();

                logger.LogError(ex.Message);

            }
        }
    }
}
