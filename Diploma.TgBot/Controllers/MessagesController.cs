using Telegram.Bot;
using TgBotLib.Core;
using TgBotLib.Core.Base;

namespace Diploma.TgBot.Controllers;

public class MessagesController : BotController
{
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IKeyboardButtonsGenerationService _keyboardButtonsGenerationService;
    
    public MessagesController(IInlineButtonsGenerationService buttonsGenerationService,
        IKeyboardButtonsGenerationService keyboardButtonsGenerationService)
    {
        _buttonsGenerationService = buttonsGenerationService;
        _keyboardButtonsGenerationService = keyboardButtonsGenerationService;
    }
    
    [Message("Test")]
    [Message(@"Test\d", isPattern: true)]
    public Task TestMessage()
    {
        _keyboardButtonsGenerationService.SetKeyboardButtons("Test", "Test1", "Buttons");
        return Client.SendTextMessageAsync(Update.GetChatId(),
            "Test message",
            replyMarkup: _keyboardButtonsGenerationService.GetButtons());
    }

    [Callback(nameof(TestCallback))]
    [Callback(@"\d", isPattern: true)]
    public Task TestCallback()
    {
        return Client.SendTextMessageAsync(Update.GetChatId(), "Test callback");
    }

    [Message("Buttons", ignoreCase: true)]
    public Task TestWithButtons()
    {
        _buttonsGenerationService.SetInlineButtons("Test", "2", "3");
        return Client.SendTextMessageAsync(Update.GetChatId(),
            "Test buttons",
            replyMarkup: _buttonsGenerationService.GetButtons());
    }

    [UnknownMessage]
    public Task TestUnknownMessage()
    {
        return Client.SendTextMessageAsync(Update.GetChatId(), "Hmm... 🤔");
    }

    [UnknownUpdate]
    public Task TestUnknownUpdate()
    {
        return Client.SendTextMessageAsync(Update.GetChatId(), "HMM... 🤔");
    }
}