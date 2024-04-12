using Telegram.Bot;
using Telegram.Bot.Types.Enums;
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
    
    [Message("start")]
    public async Task StartMessage()
    {
        await Client.SendTextMessageAsync(Update.GetChatId(),
            "Отправьте номер телефона для регистрации",
            replyMarkup: ButtonsHelper.CreateButtonWithContactRequest("Поделиться контактом"));
    }
    
    [UnknownMessage]
    public async Task ContactMessage()
    {
        HttpClient client = new HttpClient();
        long chatId = Update.GetChatId();
        
        var formData = new Dictionary<string, string>
        {
            { "chatId", chatId.ToString() },
            { "phoneNumber", Update.Message?.Contact.PhoneNumber } 
        };

        var content = new FormUrlEncodedContent(formData);

        var response = await client.PostAsync("https://localhost:7165/api/Account/Registrate", content);
    }

    [Message("Buttons", ignoreCase: true)]
    public Task TestWithButtons()
    {
        _buttonsGenerationService.SetInlineButtons("Test", "2", "3");
        return Client.SendTextMessageAsync(Update.GetChatId(),
            "Test buttons",
            replyMarkup: _buttonsGenerationService.GetButtons());
    }
}