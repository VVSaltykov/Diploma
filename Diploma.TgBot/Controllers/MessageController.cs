using Diploma.Common.Models;
using Diploma.Common.Services;
using Diploma.TgBot.Handlers;
using Diploma.TgBot.Services;
using Microsoft.AspNetCore.Mvc;
using Refit;
using Telegram.Bot;
using TgBotLib.Core;
using TgBotLib.Core.Base;

namespace Diploma.TgBot.Controllers;

public class MessageController : BotController
{
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IUsersActionsService _usersActionsService;

    private static string tittle;
    private static string text;
    private static bool isAnonymous;
    
    public MessageController(IUsersActionsService usersActionsService, IInlineButtonsGenerationService buttonsGenerationService)
    {
        _usersActionsService = usersActionsService;
        _buttonsGenerationService = buttonsGenerationService;
    }
    
    [Message("Отправить сообщение")]
    public async Task InitHandling()
    {
        _usersActionsService.HandleUser(BotContext.Update.GetChatId(), nameof(MessageController));
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), "Напишите заголовок");
    }
    
    [ActionStep(nameof(MessageController), 0)]
    public async Task FirstStep()
    {
        tittle = Update.Message.Text;
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Напишите сообщение");
    }
    
    [ActionStep(nameof(MessageController), 1)]
    public async Task SecondStep()
    {
        text = Update.Message.Text;
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы хотите отправить сообщение анонимно?");
    }

    [ActionStep(nameof(MessageController), 2)]
    public async Task ThirdStep()
    {
        try
        {
            _buttonsGenerationService.SetInlineButtons("Отправить сообщение");
            var answer = Update.Message.Text;
            if (answer == "Да") isAnonymous = true;
            if (answer == "Нет") isAnonymous = false;

            await MessageHandler.SendMessage(BotContext.Update.GetChatId(), tittle, text, isAnonymous);

            await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы отправили сообщение",
                replyMarkup: _buttonsGenerationService.GetButtons());
        }
        catch (Exception ex)
        {
            // Обработка других исключений, если необходимо
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}