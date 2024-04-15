namespace Diploma.TgBot;

public static class BotSettings
{
    public static string BotToken { get; } = Environment.GetEnvironmentVariable("BOT_TOKEN") ?? "5771058790:AAEBZTAiiSsnDN_FtQMdZDnSHyyx7Hc4mAA";
    public static string BackRoot { get; } = Environment.GetEnvironmentVariable("BACK_ROOT") ?? "https://localhost:7165";
}