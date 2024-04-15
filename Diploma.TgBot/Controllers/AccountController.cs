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

namespace Diploma.TgBot.Controllers;

public class AccountController : BotController
{
    private readonly IUsersActionsService _usersActionsService;

    private static string phoneNumber;
    private static string name;

    public AccountController(IUsersActionsService usersActionsService)
    {
        _usersActionsService = usersActionsService;
    }
    
    [Message("/start")]
    public async Task InitHandling()
    {
        _usersActionsService.HandleUser(BotContext.Update.GetChatId(), nameof(AccountController));
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), "Отправьте свой контакт",
        replyMarkup: ButtonsHelper.CreateButtonWithContactRequest("Поделиться контактом"));
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
            await RegistrationHandler.RegistrationUser(BotContext.Update.GetChatId(), phoneNumber, name,
                Update.Message.Text);
            await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы теперь зарегистрированы!");
        }
        catch (Exception ex) {}
    }
}