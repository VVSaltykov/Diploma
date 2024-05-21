using Diploma.Common.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Telegram.Bot;

namespace Diploma.TgBot.Services;

public class SignalRService
{
    private readonly HubConnection _hubConnection;
    private readonly TelegramBotClient _telegramBotClient;

    public SignalRService(TelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7165/messageHub") // Укажите правильный URL для вашего API
            .Build();

        _hubConnection.On<Messages>("ReceiveMessage", async message =>
        {
            // Логика обработки полученного сообщения
            Console.WriteLine($"Received message: {message.Tittle}");
            // Вызываем метод для отправки сообщения в телеграм боте
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
        foreach (var recepientId in message.RecepientInTelegramIds)
        {
            await _telegramBotClient.SendTextMessageAsync(recepientId, $"{message.Tittle} + {message.Text}");
        }
    }
}