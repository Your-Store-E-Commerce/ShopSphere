using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopSphere.Data.Context;

namespace ShopSphere.Web.Helper
{
    public class ApplySeeding
	{
		public static async Task SeedAsync(WebApplication app)
		{
			using var Scope = app.Services.CreateScope();

			var Service = Scope.ServiceProvider;

			var _context = Service.GetRequiredService<ShopSphere_DbContext>();


			var loggerfactory = Service.GetRequiredService<ILoggerFactory>();

			try
			{
				await _context.Database.MigrateAsync();

				await ShopSphereContextSeed.ApplySeedingAsync(_context);


				
			}
			catch (Exception ex)
			{
				var logger = loggerfactory.CreateLogger<Program>();

				logger.LogError(ex.Message);

			}
		}
	}
}