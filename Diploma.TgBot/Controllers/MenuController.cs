using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Diploma.TgBot.Services;
using Diploma.TgBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotLib.Core;
using TgBotLib.Core.Base;

namespace Diploma.TgBot.Controllers;

public class MenuController : BotController
{
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IKeyboardButtonsGenerationService _keyboardButtonsGenerationService;

    public MenuController(IInlineButtonsGenerationService buttonsGenerationService,
        IKeyboardButtonsGenerationService keyboardButtonsGenerationService)
    {
        _buttonsGenerationService = buttonsGenerationService;
        _keyboardButtonsGenerationService = keyboardButtonsGenerationService;
    }
}