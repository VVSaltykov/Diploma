using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Diploma.TgBot.Handlers;
using Diploma.TgBot.Services;
using Diploma.TgBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TgBotLib.Core;
using TgBotLib.Core.Base;

namespace Diploma.TgBot.Controllers;

public class MessageToProfessorController : BotController
{
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IUsersActionsService _usersActionsService;
    
    private static string tittle;
    private static string text;
    private static string professorName;
    
    public MessageToProfessorController(IUsersActionsService usersActionsService, IInlineButtonsGenerationService buttonsGenerationService)
    {
        _usersActionsService = usersActionsService;
        _buttonsGenerationService = buttonsGenerationService;
    }
    
    [Message("Отправить сообщение преподавателю")]
    public async Task InitHandling()
    {
        _usersActionsService.HandleUser(BotContext.Update.GetChatId(), nameof(MessageToProfessorController));
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), "Напишите заголовок");
    }
    
    [ActionStep(nameof(MessageToProfessorController), 0)]
    public async Task FirstStep()
    {
        tittle = Update.Message.Text;
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Напишите сообщение");
    }
    
    [ActionStep(nameof(MessageToProfessorController), 1)]
    public async Task SecondStep()
    {
        text = Update.Message.Text;
        
        UserService userService = SingletonService.GetUserService();

        var webUsers = await userService.GetWebUsers();

        var professors = webUsers.Where(u => u.Role == Role.Professor).ToList();
        
        var buttons = professors.Select(professor => new KeyboardButton(professor.Name)).ToArray();

        var professorsReplyMarkup = new ReplyKeyboardMarkup(buttons)
        {
            ResizeKeyboard = true
        };
        
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Выберите преподавателя, которому хотите отправить сообщение",
            replyMarkup: professorsReplyMarkup);
    }

    [ActionStep(nameof(MessageToProfessorController), 2)]
    public async Task ThirdStep()
    {
        try
        {
            professorName = Update.Message.Text;
            await MessageHandler.SendToProfessorMessage(BotContext.Update.GetChatId(), tittle, text, professorName);

            await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы отправили сообщение",
                replyMarkup: Buttons.StudentButtons());
        }
        catch (Exception ex)
        {
            // Обработка других исключений, если необходимо
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}