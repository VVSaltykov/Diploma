using System.IO.Compression;
using Diploma.Common.Models;
using Diploma.Common.Services;
using Diploma.Common.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

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
        FilesService filesService = SingletonService.GetFilesService();
        var user = await userService.GetUserById(message.UserId);
        var files = await filesService.GetFiles(message.FilesIds);
        var senderInfo = user != null ? $"{user.Name}" : "Unknown User";
        
        foreach (var recepientId in message.RecepientInTelegramIds)
        {
            var formattedMessage = $"**{message.Tittle}**\n \n" +
                                   $"**Отправитель:**{senderInfo}\n \n" +
                                   $"**Дата и время:**{message.DateTime}\n \n" +
                                   $"**Текст:** \n" +
                                   $"{message.Text}";
            
            byte[] zipBytes = await ZipFileCreator.CreateZipFile(files);
            
            using (MemoryStream zipStream = new MemoryStream(zipBytes))
            {
                var fileName = "Файлы.zip";
                var inputMediaDocument = new InputFileStream(zipStream, fileName);

                await _telegramBotClient.SendDocumentAsync(recepientId, inputMediaDocument, caption: formattedMessage,parseMode: ParseMode.Markdown);
            }
        }
    }
}