using Telegram.Bot;
using TgBotLib.Core;
using TgBotLib.Core.Base;

namespace Diploma.TgBot.Controllers;

public class TestController : BotController
{
    private readonly IUsersActionsService _usersActionsService;

    public TestController(IUsersActionsService usersActionsService)
    {
        _usersActionsService = usersActionsService;
    }

    [Message("Init handling")]
    public async Task InitHandling()
    {
        _usersActionsService.HandleUser(BotContext.Update.GetChatId(), nameof(TestController));
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), "Handling inited");
    }

    [ActionStep(nameof(TestController), 0)]
    public async Task FirstStep()
    {
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"First step {BotContext.Update.GetMessageText()}");
    }

    [ActionStep(nameof(TestController), 1)]
    public async Task SecondStep()
    {
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Second step {BotContext.Update.GetMessageText()}");
    }

    [ActionStep(nameof(TestController), 2)]
    public async Task ThirdStep()
    {
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Third step {BotContext.Update.GetMessageText()}");
    }
}