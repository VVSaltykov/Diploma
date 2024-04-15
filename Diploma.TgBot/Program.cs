using TgBotLib.Core;

namespace Diploma.TgBot;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddBotLibCore(BotSettings.BotToken);

        var app = builder.Build();

        app.Run();
    }
}