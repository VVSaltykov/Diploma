using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Diploma.TgBot.Handlers;
using Diploma.TgBot.Services;
using Diploma.TgBot.UI;
using Microsoft.AspNetCore.Mvc;
using Refit;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
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
        var buttons = new List<KeyboardButton>
        {
            new ("Да"),
            new ("Нет")
        };

        var replyMarkup = new ReplyKeyboardMarkup(buttons.Select(b => new[] { b }))
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы хотите отправить сообщение анонимно?",
            replyMarkup: replyMarkup);
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
            
            AccountService accountService = SingletonService.GetAccountService();
            var User = await accountService.Read(BotContext.Update.GetChatId());

            if (User.Role == Role.Graduate)
            {
                await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы отправили сообщение",
                    replyMarkup: Buttons.GraduateButtons());
            }
            if (User.Role == Role.Applicant)
            {
                await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы отправили сообщение",
                    replyMarkup: Buttons.ApplicantButtons());
            }
            if (User.Role == Role.Student)
            {
                await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы отправили сообщение",
                    replyMarkup: Buttons.StudentButtons());
            }
        }
        catch (Exception ex)
        {
            // Обработка других исключений, если необходимо
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}