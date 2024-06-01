using System.IO.Compression;
using Diploma.Common.Models;
using Diploma.Common.Services;
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
        
        
        string tempDir = Path.Combine(Path.GetTempPath(), "TelegramFiles");
        Directory.CreateDirectory(tempDir);

        // Архивируем файлы
        string zipFilePath = Path.Combine(tempDir, "files.zip");
        using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Create))
        {
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
            {
                foreach (var file in files)
                {
                    ZipArchiveEntry entry = archive.CreateEntry(file.FileName);
                    using (Stream entryStream = entry.Open())
                    {
                        await entryStream.WriteAsync(file.FileData, 0, file.FileData.Length);
                    }
                }
            }
        }
        
        foreach (var recepientId in message.RecepientInTelegramIds)
        {
            var formattedMessage = $"**{message.Tittle}**\n \n" +
                                   $"**Отправитель:**{senderInfo}\n \n" +
                                   $"**Дата и время:**{message.DateTime}\n \n" +
                                   $"**Текст:** \n" +
                                   $"{message.Text}";
            
            using (var stream = File.OpenRead(zipFilePath))
            {
                // Создаем объект InputMediaDocument для отправки
                var fileName = "files.zip"; // Задайте имя файла
                var inputMediaDocument = new InputFileStream(stream, fileName);

                // Отправляем документ с сообщением
                await _telegramBotClient.SendDocumentAsync(recepientId, inputMediaDocument, caption: formattedMessage,parseMode: ParseMode.Markdown);
            }
        }
        Directory.Delete(tempDir, true);
    }
}