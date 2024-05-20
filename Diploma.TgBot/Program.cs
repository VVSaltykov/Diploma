using Diploma.TgBot.Services;
using TgBotLib.Core;

namespace Diploma.TgBot;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddBotLibCore(BotSettings.BotToken);
        
        builder.Services.AddSingleton<SignalRService>();

        var app = builder.Build();
        
        app.MapGet("/", (HttpContext context) =>
        {
            context.Response.StatusCode = 204; // No Content
            return Task.CompletedTask;
        });
        
        var signalRService = app.Services.GetRequiredService<SignalRService>();
        signalRService.StartAsync().Wait();
        
        app.Run();
    }
}