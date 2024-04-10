using TgBotLib.Core;

namespace Diploma.TgBot;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddBotLibCore("5771058790:AAHbHiuZxXkoOizkXW8PXMxS3jknCySI_7E");

        var app = builder.Build();

        app.Run();
    }
}