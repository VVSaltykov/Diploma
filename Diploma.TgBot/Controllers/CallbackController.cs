using Telegram.Bot;
using TgBotLib.Core;
using TgBotLib.Core.Base;

namespace Diploma.TgBot.Controllers;

public class CallbackController : BotController
{
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IKeyboardButtonsGenerationService _keyboardButtonsGenerationService;

    public CallbackController(IInlineButtonsGenerationService buttonsGenerationService,
        IKeyboardButtonsGenerationService keyboardButtonsGenerationService)
    {
        _buttonsGenerationService = _buttonsGenerationService;
        _keyboardButtonsGenerationService = keyboardButtonsGenerationService;
    }
    
    [Callback(nameof(ContactCallback))]
    public async Task ContactCallback(long chatId)
    {
        HttpClient client = new HttpClient();
        var response = await client.PostAsJsonAsync("https://localhost:7165/api/Account", new { chatId, Update.Message?.Contact });
    }
}