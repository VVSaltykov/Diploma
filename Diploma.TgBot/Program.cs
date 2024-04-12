using TgBotLib.Core;

namespace Diploma.TgBot;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddBotLibCore("5009457163:AAHK15woZY6hEh3KI2XDMP7m5VzxOjc0-k4");

        var app = builder.Build();

        app.Run();
    }
}