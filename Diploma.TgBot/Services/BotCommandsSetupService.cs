using Telegram.Bot;
using Telegram.Bot.Types;

namespace Diploma.TgBot.Services;

public class BotCommandsSetupService : IHostedService
{
    private readonly TelegramBotClient _botClient;

    public BotCommandsSetupService(TelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var commands = new[]
        {
            new BotCommand { Command = "start", Description = "На главную" },
            new BotCommand { Command = "help", Description = "Получить помощь" }
        };

        await _botClient.SetMyCommandsAsync(commands, cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}