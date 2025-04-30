using Microsoft.EntityFrameworkCore;
using ShopSphere.Data.Context;
using ShopSphere.Data.Interfaces;
using ShopSphere.Data.Repositories.Implementations;
using ShopSphere.Data.Repositories.Interfaces;
using ShopSphere.Data.UnitOfWork;
using ShopSphere.Services.Implementations;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Helper;
using ShopSphere.Web.Mapper;
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
            builder.Services.AddScoped(typeof(IBasketRepository) , typeof(BasketRepository));
            builder.Services.AddScoped(typeof(IPaymentServices), typeof(PaymentServices));
            builder.Services.AddScoped(typeof(IOrderServices) , typeof(OrderServices));
            builder.Services.AddScoped(typeof(IProductsServices), typeof(ProductsServices));
            builder.Services.AddScoped(typeof(IBasketServices), typeof(BasketServices));

            builder.Services.AddAutoMapper(typeof(MappingProfile));


            var app = builder.Build();

            await DatabaseInitializer.InitializeDatabaseAsync(app);
			await ApplySeeding.SeedAsync(app);
		
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
