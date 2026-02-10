using Microsoft.AspNetCore.Connections;
using MobileRecharge.Application.Interfaces;
using MobileRecharge.Application.Services;
//using MobileRecharge.Infrastructure.Db;
//using MobileRecharge.Infrastructure.Interfaces;
//using MobileRecharge.Infrastructure.Repositories;
namespace MobileRecharge.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            string connectionString =
            builder.Configuration.GetConnectionString("DefaultConnection");

            // DB Connection Factory
            //builder.Services.AddSingleton(new DbConnectionFactory(connectionString));

            //// Repositories
            //builder.Services.AddScoped<IRechargeRepository, RechargeRepository>();
            //builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Services
            builder.Services.AddScoped<IRechargeService, RechargeService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();


            var app = builder.Build();

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
