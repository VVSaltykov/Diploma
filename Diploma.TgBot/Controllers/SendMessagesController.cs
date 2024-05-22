using System.Collections.Concurrent;
using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Diploma.TgBot.Data;
using Diploma.TgBot.Services;
using Diploma.TgBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotLib.Core;
using TgBotLib.Core.Base;

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
    
    [Message("Мои сообщения", ignoreCase: true)]
    public async Task SendMessages()
    {
        MessagesService messagesService = SingletonService.GetMessagesService();
        var messages = await messagesService.GetUserMessages(Update.GetChatId());

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
            userMessagesState.CurrentMessageIndex = (userMessagesState.CurrentMessageIndex - 1 + userMessagesState.Messages.Count) % userMessagesState.Messages.Count;
            await ShowCurrentMessage(userMessagesState);
        }
    }

    [Callback("next", ignoreCase: true)]
    public async Task ShowNextMessage()
    {
        var userMessagesState = UserMessagesStateService.GetUserMessagesState(Update.GetChatId());

        if (userMessagesState != null && userMessagesState.Messages.Count > 0)
        {
            userMessagesState.CurrentMessageIndex = (userMessagesState.CurrentMessageIndex + 1) % userMessagesState.Messages.Count;
            await ShowCurrentMessage(userMessagesState);
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
        var currentMessage = state.Messages[state.CurrentMessageIndex];
        
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Назад", "previous"),
                InlineKeyboardButton.WithCallbackData("Вперед", "next")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Домой", "tohome")
            }
        });
        if (state.LastMessageId == 0)
        {
            var removeKeyboardMessage = await Client.SendTextMessageAsync(Update.GetChatId(), ".", replyMarkup: new ReplyKeyboardRemove());
            await Client.DeleteMessageAsync(Update.GetChatId(), removeKeyboardMessage.MessageId);
            var sentMessage = await Client.SendTextMessageAsync(Update.GetChatId(), currentMessage.Text, replyMarkup: inlineKeyboard);
            state.LastMessageId = sentMessage.MessageId;
        }
        else
        {
            await Client.EditMessageTextAsync(Update.GetChatId(), state.LastMessageId, currentMessage.Text, replyMarkup: inlineKeyboard);
        }
    }
}