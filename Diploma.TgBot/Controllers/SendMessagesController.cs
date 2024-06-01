using System.Collections.Concurrent;
using System.IO.Compression;
using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Diploma.Common.Utils;
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

public class SendMessagesController : BotController
{ 
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IKeyboardButtonsGenerationService _keyboardButtonsGenerationService;
    
    public SendMessagesController(IInlineButtonsGenerationService buttonsGenerationService, IKeyboardButtonsGenerationService keyboardButtonsGenerationService)
    {
        _buttonsGenerationService = buttonsGenerationService;
        _keyboardButtonsGenerationService = keyboardButtonsGenerationService;
    }
    
    [Message("Мои сообщения", MessageType.Text, ignoreCase: true)]
    public async Task SendMessages()
    {
        MessagesService messagesService = SingletonService.GetMessagesService();
        var messages = await messagesService.GetUserMessages(Update.GetChatId());
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

    [Callback("previous", ignoreCase: true)]
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

    [Callback("next", ignoreCase: true)]
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
    
    [Callback("tohome", ignoreCase: true)]
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

        if (currentMessage.FilesIds.Any())
        {
            var files = await filesService.GetFiles(currentMessage.FilesIds);
            byte[] zipBytes = await ZipFileCreator.CreateZipFile(files);
            
            using (MemoryStream zipStream = new MemoryStream(zipBytes))
            {
                var fileName = "Файлы.zip";
                var inputMediaDocument = new InputFileStream(zipStream, fileName);

                await Client.SendDocumentAsync(Update.GetChatId(), inputMediaDocument, caption: formattedMessage, replyMarkup: inlineKeyboard, parseMode: ParseMode.Markdown);
            }
        }
        else
        {
            await Client.SendTextMessageAsync(Update.GetChatId(), formattedMessage, replyMarkup: inlineKeyboard, parseMode: ParseMode.Markdown);
        }
    }
}