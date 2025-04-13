using Microsoft.EntityFrameworkCore;
using ShopSphere.Data.Context;
using ShopSphere.Data.Interfaces;
using ShopSphere.Data.Repositories.Implementations;
using ShopSphere.Data.Repositories.Interfaces;
using ShopSphere.Data.UnitOfWork;
using ShopSphere.Web.Helper;
using StackExchange.Redis;

namespace ShopSphere.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ShopSphere_DbContext>(options =>

            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
         );

            builder.Services.AddSingleton<IConnectionMultiplexer>(Service =>
            {
                var conect = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(conect);
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped(typeof(IUnitOfWork) , typeof(UnitOfWork));   

            var app = builder.Build();

            await DatabaseInitializer.InitializeDatabaseAsync(app);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
