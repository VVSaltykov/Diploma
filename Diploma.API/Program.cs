using Diploma.API.Repositories;
using Diploma.API.Services;
using Diploma.Common.Services;
using Diploma.Common.ServicesForWeb;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace Diploma.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("CompetitionsWebDbConnection"));
        });

        builder.Services.AddAuthentication("Cookies")
            .AddCookie("Cookies", options =>
            {
                options.Cookie.Name = "token";
                options.Cookie.Domain = "localhost:7099";
                options.Cookie.Path = "/"; // Устанавливаем путь cookie
            });
        builder.Services.AddAuthorization();
        
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins("http://localhost:5276")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        
        builder.Services.AddMemoryCache();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddTransient<UserRepository>();
        builder.Services.AddTransient<GroupRepository>();
        builder.Services.AddTransient<MessagesRepository>();
        builder.Services.AddSingleton<SessionService>();
        

        var app = builder.Build();
        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation");
            c.RoutePrefix = "swagger";
        });

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        app.UseCors();

        app.Run();
    }
}