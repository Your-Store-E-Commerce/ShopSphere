//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using ShopSphere.Data.Context;

//namespace ShopSphere.Web.Helper
//{
//    public class ApplySeeding
//	{
//		public static async Task SeedAsync(WebApplication app)
//		{
//			using var Scope = app.Services.CreateScope();

//			var Service = Scope.ServiceProvider;

//			var _context = Service.GetRequiredService<ShopSphere_DbContext>();


//			var loggerfactory = Service.GetRequiredService<ILoggerFactory>();

//			try
//			{
//				await _context.Database.MigrateAsync();

//				await ShopSphereContextSeed.ApplySeedingAsync(_context);



//			}
//			catch (Exception ex)
//			{
//				var logger = loggerfactory.CreateLogger<Program>();

//				logger.LogError(ex.Message);

//			}
//		}
//	}
//}


using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopSphere.Data.Context;

namespace ShopSphere.Web.Helper
{
	public class ApplySeeding
	{
		public static async Task SeedAsync(WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;

			var context = services.GetRequiredService<ShopSphere_DbContext>();
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			var logger = loggerFactory.CreateLogger<ApplySeeding>();

			try
			{
				logger.LogInformation("Applying migrations...");
				await context.Database.MigrateAsync();
				logger.LogInformation("Migrations applied successfully.");

				logger.LogInformation("Starting seeding process...");
				await ShopSphereContextSeed.ApplySeedingAsync(context);
				logger.LogInformation("Seeding process completed.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occurred during migration or seeding.");
			}
		}
	}
}
