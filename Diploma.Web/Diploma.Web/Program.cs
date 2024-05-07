using Diploma.API.Services;
using Diploma.Common.Interfaces;
using Diploma.Common.Services;
using Diploma.Common.Utils;
using Diploma.Web.Services;
using Diploma.Web.Utils;
using Microsoft.AspNetCore.Components.Authorization;

namespace Diploma.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            
            builder.Services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.Cookie.Name = "token";
                    options.Cookie.Domain = "localhost:7110";
                    options.Cookie.Path = "/"; // Устанавливаем путь cookie
                });
            builder.Services.AddAuthorization();
            
            builder.Services.AddHttpClient<IAccountService, AccountService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7165");
            });
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<SessionCacheService>();
            builder.Services.AddScoped<CookieService>();
            builder.Services.AddScoped<CookiesService>();
            builder.Services.AddTransient<SessionService>();
            builder.Services.AddScoped<SessionsService>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CookieAuthenticationStateProvider>());
            builder.Services.AddScoped<CookieAuthenticationStateProvider>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}
