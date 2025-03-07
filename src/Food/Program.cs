using DataAccess;
using Food.Repositories;
using Food.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Food
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddDbContext<FoodDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Food")));
            // add Repository
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IContactRepository, ContactRepository>();

            builder.Services.AddScoped<OrderDAO>();
            builder.Services.AddScoped<PaymentService>();

            // Cấu hình Cookie Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Users/Login";
                    options.AccessDeniedPath = "/Users/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    options.SlidingExpiration = true;  // Optional: This will reset the expiration time each time a user is active

                });
            builder.Services.AddScoped<IEmailService, EmailService>();
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapHub<SignalRService>("/SignalRHub");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                // Cấu hình chuyển hướng mặc định đến trang Menu
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/Users/Default");
                    return Task.CompletedTask;
                });
            });
            app.MapRazorPages();


            app.Run();
        }
    }
}
