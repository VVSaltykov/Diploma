using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotLib.Core;
using TgBotLib.Core.Base;

namespace Diploma.TgBot.Controllers;

public class InfoController : BotController
{ 
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IKeyboardButtonsGenerationService _keyboardButtonsGenerationService;
    
    public InfoController(IInlineButtonsGenerationService buttonsGenerationService, IKeyboardButtonsGenerationService keyboardButtonsGenerationService)
    {
        _buttonsGenerationService = buttonsGenerationService;
        _keyboardButtonsGenerationService = keyboardButtonsGenerationService;
    }
    
    [Message("Полезные ресурсы", MessageType.Text, ignoreCase: true)]
    public async Task SendMessages()
    {
        long chatId = BotContext.Update.GetChatId();

        // Создаем кнопки с полезными ресурсами
        var buttons = new[]
        {
            new[]
            {
                InlineKeyboardButton.WithUrl("Сайт МАИ", "https://mai.ru/")
            },
            new[]
            {
                InlineKeyboardButton.WithUrl("Деканат 6-го факультета", "https://vk.com/aerocosmos6")
            },
            new[]
            {
                InlineKeyboardButton.WithUrl("Институт №6", "https://institutes.mai.ru/space/?referer=https%3A%2F%2Faway.vk.com%2F"),
            },
            new[]
            {
                InlineKeyboardButton.WithUrl("Кафедра 609 в ВК", "https://vk.com/space609")
            },
        };

        // Создаем клавиатуру с кнопками
        var replyMarkup = new InlineKeyboardMarkup(buttons);

        // Отправляем сообщение с кнопками
        await Client.SendTextMessageAsync(chatId, "Полезные ресурсы:", replyMarkup: replyMarkup);
    }
}