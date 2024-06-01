namespace Diploma.TgBot;

public static class BotSettings
{
    public static string BotToken { get; } = Environment.GetEnvironmentVariable("BOT_TOKEN") ?? "7446573086:AAG6GBnwh_crkZ15vTf037grxGCx5bQ-4vA";
    public static string BackRoot { get; } = Environment.GetEnvironmentVariable("BACK_ROOT") ?? "https://localhost:7165";
}