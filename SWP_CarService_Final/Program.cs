using Microsoft.AspNetCore.Authentication.Cookies;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(20);
            });

            //Add scrope for services
            builder.Services.AddScoped<DBContext>();
            builder.Services.AddScoped<UserServices>();
            builder.Services.AddScoped<AppointmentService>();
            builder.Services.AddScoped<TaskService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<UserAccountServices>();
            builder.Services.AddScoped<CustomerAccountService>();
            builder.Services.AddScoped<TeamService>();
            builder.Services.AddScoped<TeamMemberService>();




            builder.Services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                    option =>
                    {
                        option.LoginPath = "/Home/login";
                        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                        option.AccessDeniedPath = "/Home/logout";
                    }
                );

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

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

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                  name: "User",
                  areaName: "User",
                  pattern: "{user}/{controller=Home}/{action=Login}/{id?}"
                );

                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

            });

            app.Run();
        }
    }
}