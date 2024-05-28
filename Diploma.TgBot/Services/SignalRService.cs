using Diploma.Common.Models;
using Diploma.Common.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Diploma.TgBot.Services;

public class SignalRService
{
    private readonly HubConnection _hubConnection;
    private readonly TelegramBotClient _telegramBotClient;

    public SignalRService(TelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7165/messageHub")
            .Build();

        _hubConnection.On<Messages>("ReceiveMessage", async message =>
        {
            await SendMessageToTelegramUser(message);
        });
    }

    public async Task StartAsync()
    {
        await _hubConnection.StartAsync();
        Console.WriteLine("SignalR connected.");
    }

    private async Task SendMessageToTelegramUser(Messages message)
    {
        UserService userService = SingletonService.GetUserService();
        var user = await userService.GetUserById(message.UserId);
        var senderInfo = user != null ? $"{user.Name}" : "Unknown User";
        foreach (var recepientId in message.RecepientInTelegramIds)
        {
            var formattedMessage = $"**{message.Tittle}**\n \n" +
                                   $"**Отправитель:**{senderInfo}\n \n" +
                                   $"**Дата и время:**{message.DateTime}\n \n" +
                                   $"**Текст:** \n" +
                                   $"{message.Text}";
            await _telegramBotClient.SendTextMessageAsync(recepientId, formattedMessage, parseMode: ParseMode.Markdown);
        }
    }
}