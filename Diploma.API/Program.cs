using Diploma.API.Data;
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
            options.UseNpgsql(builder.Configuration.GetConnectionString("MAIDbConnection"));
        });

        builder.Services.AddSignalR();
        
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
                builder.WithOrigins("https://localhost:7099")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        
        builder.Services.AddMemoryCache();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
        

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddTransient<UserRepository>();
        builder.Services.AddTransient<GroupRepository>();
        builder.Services.AddTransient<MessagesRepository>();
        builder.Services.AddTransient<SaltRepository>();
        builder.Services.AddSingleton<SessionService>();
        builder.Services.AddTransient<SaltService>();
        

        var app = builder.Build();
        
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation");
            c.RoutePrefix = "swagger";
        });

        app.UseHttpsRedirection();

        app.UseCors(); // CORS should be used before routing

        app.UseRouting(); // UseRouting should be called before other middlewares

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<MessageHub>("/messageHub");
        });

        app.Run();
    }
}