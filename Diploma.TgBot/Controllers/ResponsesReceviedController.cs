using System.IO.Compression;
using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Diploma.TgBot.Data;
using Diploma.TgBot.Services;
using Diploma.TgBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotLib.Core;
using TgBotLib.Core.Base;
using File = System.IO.File;

namespace Diploma.TgBot.Controllers;

public class ResponsesReceviedController : BotController
{ 
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IKeyboardButtonsGenerationService _keyboardButtonsGenerationService;

    public ResponsesReceviedController(IInlineButtonsGenerationService buttonsGenerationService, IKeyboardButtonsGenerationService keyboardButtonsGenerationService)
    {
        _buttonsGenerationService = buttonsGenerationService;
        _keyboardButtonsGenerationService = keyboardButtonsGenerationService;
    }
    
    [Message("Полученные ответы", MessageType.Text, ignoreCase: true)]
    public async Task SendMessages()
    {
        MessagesService messagesService = SingletonService.GetMessagesService();
        
        var messages = await messagesService.GetMessagesForUser(Update.GetChatId());
        messages = messages.OrderByDescending(msg => msg.DateTime).ToList();

        if (messages.Count > 0)
        {
            var userMessagesState = new UserMessagesState
            {
                Messages = messages,
                CurrentMessageIndex = 0
            };

            UserMessagesStateService.SetUserMessagesState(Update.GetChatId(), userMessagesState);

            await ShowCurrentMessage(userMessagesState);
        }
        else
        {
            await Client.SendTextMessageAsync(Update.GetChatId(), "У вас нет сообщений.");
        }
    }
    
    [Callback("previousResponse", ignoreCase: true)]
    public async Task ShowPreviousMessage()
    {
        var userMessagesState = UserMessagesStateService.GetUserMessagesState(Update.GetChatId());

        if (userMessagesState != null && userMessagesState.Messages.Count > 0)
        {
            if (userMessagesState.CurrentMessageIndex < userMessagesState.Messages.Count - 1)
            {
                userMessagesState.CurrentMessageIndex++;
                await ShowCurrentMessage(userMessagesState);
            }
        }
    }

    [Callback("nextResponse", ignoreCase: true)]
    public async Task ShowNextMessage()
    {
        var userMessagesState = UserMessagesStateService.GetUserMessagesState(Update.GetChatId());

        if (userMessagesState != null && userMessagesState.Messages.Count > 0)
        {
            if (userMessagesState.CurrentMessageIndex > 0)
            {
                userMessagesState.CurrentMessageIndex--;
                await ShowCurrentMessage(userMessagesState);
            }
        }
    }
    
    [Callback("tohomeResponse", ignoreCase: true)]
    public async Task ToHome()
    {
        AccountService accountService = SingletonService.GetAccountService();
        var User = await accountService.Read(BotContext.Update.GetChatId());

        if (User.Role == Role.Graduate)
        {
            await Client.DeleteMessageAsync(Update.GetChatId(), BotContext.Update.CallbackQuery.Message.MessageId);
            await Client.SendTextMessageAsync(Update.GetChatId(), "Выберите действие", replyMarkup: Buttons.GraduateButtons());
        }
        if (User.Role == Role.Applicant)
        {
            await Client.DeleteMessageAsync(Update.GetChatId(), BotContext.Update.CallbackQuery.Message.MessageId);
            await Client.SendTextMessageAsync(Update.GetChatId(), "Выберите действие", replyMarkup: Buttons.ApplicantButtons());
        }
        if (User.Role == Role.Student)
        {
            await Client.DeleteMessageAsync(Update.GetChatId(), BotContext.Update.CallbackQuery.Message.MessageId);
            await Client.SendTextMessageAsync(Update.GetChatId(), "Выберите действие", replyMarkup: Buttons.StudentButtons());
        }
    }

    private async Task ShowCurrentMessage(UserMessagesState state)
    {
        FilesService filesService = SingletonService.GetFilesService();
        var currentMessage = state.Messages[state.CurrentMessageIndex];
        
        var senderInfo = currentMessage.User != null ? $"**Отправитель:** {currentMessage.User.Name} ({currentMessage.User.Id})" : "Unknown User";

        var formattedMessage = $"**{currentMessage.Tittle}**\n\n" +
                               $"{senderInfo}\n\n" +
                               $"**Дата и время:** {currentMessage.DateTime}\n\n" +
                               $"**Текст:**\n{currentMessage.Text}";

        // Создаем пустой ZIP-архив
        string zipFilePath = null;
        if (currentMessage.FilesIds.Any())
        {
            var files = await filesService.GetFiles(currentMessage.FilesIds);
            string tempDir = Path.Combine(Path.GetTempPath(), "TelegramFiles");
            Directory.CreateDirectory(tempDir);

            zipFilePath = Path.Combine(tempDir, "files.zip");
            using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    // Добавляем файлы в ZIP-архив
                    foreach (var file in files)
                    {
                        // Добавляем файл в архив
                        ZipArchiveEntry entry = archive.CreateEntry(file.FileName);
                        using (Stream entryStream = entry.Open())
                        {
                            await entryStream.WriteAsync(file.FileData, 0, file.FileData.Length);
                        }
                    }
                }
            }
        }

        // Создаем InlineKeyboardMarkup
        var inlineKeyboardRows = new List<InlineKeyboardButton[]>
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Предыдущее", "previous")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Домой", "tohome")
            }
        };
        
        if (state.CurrentMessageIndex < state.Messages.Count - 1 && state.CurrentMessageIndex != 0)
        {
            inlineKeyboardRows.Insert(0, new []
            {
                InlineKeyboardButton.WithCallbackData("Следующее", "next")
            });
        }

        var inlineKeyboard = new InlineKeyboardMarkup(inlineKeyboardRows);

        // Отправляем сообщение с файлом (если он есть) или без него
        if (zipFilePath != null)
        {
            using (var stream = File.OpenRead(zipFilePath))
            {
                // Создаем объект InputMediaDocument для отправки ZIP-архива
                var fileName = "files.zip"; // Задайте имя файла
                var inputMediaDocument = new InputFileStream(stream, fileName);

                // Отправляем документ с сообщением и клавиатурой
                await Client.SendDocumentAsync(Update.GetChatId(), inputMediaDocument, caption: formattedMessage, replyMarkup: inlineKeyboard, parseMode: ParseMode.Markdown);
            }
        }
        else
        {
            // Отправляем сообщение без файла, только с клавиатурой
            await Client.SendTextMessageAsync(Update.GetChatId(), formattedMessage, replyMarkup: inlineKeyboard, parseMode: ParseMode.Markdown);
        }

        // Удаляем временную директорию, если она создавалась
        if (zipFilePath != null)
        {
            Directory.Delete(Path.GetDirectoryName(zipFilePath), true);
        }
    }
}