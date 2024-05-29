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
        builder.Services.AddSingleton<BotCommandsSetupService>();

        var app = builder.Build();
        
        var signalRService = app.Services.GetRequiredService<SignalRService>();
        signalRService.StartAsync().Wait();
        
        app.Run();
    }
}