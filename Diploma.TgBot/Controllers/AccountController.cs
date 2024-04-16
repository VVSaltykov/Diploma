using System.Net;
using System.Text;
using System.Text.Json;
using Diploma.API.Repositories;
using Diploma.Common.Models;
using Diploma.Common.Services;
using Diploma.TgBot.Handlers;
using Diploma.TgBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBotLib.Core;
using TgBotLib.Core.Base;
using User = Diploma.Common.Models.User;

namespace Diploma.TgBot.Controllers;

public class AccountController : BotController
{
    private readonly IUsersActionsService _usersActionsService;
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;

    private static User? User;
    private static string phoneNumber;
    private static string name;

    public AccountController(IUsersActionsService usersActionsService, IInlineButtonsGenerationService buttonsGenerationService)
    {
        _usersActionsService = usersActionsService;
        _buttonsGenerationService = buttonsGenerationService;
    }
    
    [Message("/start")]
    public async Task InitHandling()
    {
        try
        {
            AccountService accountService = SingletonService.GetAccountService();
            User = await accountService.Read(BotContext.Update.GetChatId());
            if (User == null)
            {
                _usersActionsService.HandleUser(BotContext.Update.GetChatId(), nameof(AccountController));
                await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), "Отправьте свой контакт",
                    replyMarkup: ButtonsHelper.CreateButtonWithContactRequest("Поделиться контактом"));
            }
            else
            {
                _buttonsGenerationService.SetInlineButtons("Отправить сообщение");
                await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), "Отправьте сообщение",
                    replyMarkup: _buttonsGenerationService.GetButtons());
            } 
        }
        catch (Exception ex)
        {
            // Обработка других исключений
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    
    [ActionStep(nameof(AccountController), 0)]
    public async Task FirstStep()
    {
        phoneNumber = Update.Message?.Contact.PhoneNumber;
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Как вас зовут?");
    }

    [ActionStep(nameof(AccountController), 1)]
    public async Task SecondStep()
    {
        name = Update.Message.Text;
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Какая у вас группа?");
    }

    [ActionStep(nameof(AccountController), 2)]
    public async Task ThirdStep()
    {
        try
        {
            _buttonsGenerationService.SetInlineButtons("Отправить сообщение");
            await RegistrationHandler.RegistrationUser(BotContext.Update.GetChatId(), phoneNumber, name,
                Update.Message.Text);
            await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы теперь зарегистрированы!",
                replyMarkup: _buttonsGenerationService.GetButtons());
        }
        catch (Exception ex) {}
    }
}