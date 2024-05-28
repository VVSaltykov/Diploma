using System.Collections.Concurrent;
using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Diploma.TgBot.Data;
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

    private static ConcurrentDictionary<long, UserSentMessageState> userStates = new ConcurrentDictionary<long, UserSentMessageState>();

    public MessageController(IUsersActionsService usersActionsService, IInlineButtonsGenerationService buttonsGenerationService)
    {
        _usersActionsService = usersActionsService;
        _buttonsGenerationService = buttonsGenerationService;
    }
    
    [Message]
    public async Task ToDelete()
    {
        await Client.SendTextMessageAsync(Update.Message.Chat.Id, "asdadsasd");
    }

    [Message("Отправить сообщение")]
    public async Task InitHandling()
    {
        long chatId = BotContext.Update.GetChatId();
        _usersActionsService.HandleUser(chatId, nameof(MessageController));
        
        userStates[chatId] = new UserSentMessageState(); // Инициализация состояния пользователя
        
        await Client.SendTextMessageAsync(chatId, "Напишите заголовок");
    }
    
    [ActionStep(nameof(MessageController), 0)]
    public async Task FirstStep()
    {
        long chatId = BotContext.Update.GetChatId();
        
        if (userStates.TryGetValue(chatId, out var userState))
        {
            userState.Title = Update.Message.Text;
        }
        
        await Client.SendTextMessageAsync(chatId, "Напишите сообщение");
    }
    
    [ActionStep(nameof(MessageController), 1)]
    public async Task SecondStep()
    {
        long chatId = BotContext.Update.GetChatId();
        
        if (userStates.TryGetValue(chatId, out var userState))
        {
            userState.Text = Update.Message.Text;
        }
        
        var buttons = new List<KeyboardButton>
        {
            new("Да"),
            new("Нет")
        };

        var replyMarkup = new ReplyKeyboardMarkup(buttons.Select(b => new[] { b }))
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
        
        await Client.SendTextMessageAsync(chatId, "Вы хотите отправить сообщение анонимно?", replyMarkup: replyMarkup);
    }

    [ActionStep(nameof(MessageController), 2)]
    public async Task ThirdStep()
    {
        long chatId = BotContext.Update.GetChatId();
        
        if (userStates.TryGetValue(chatId, out var userState))
        {
            var answer = Update.Message.Text;
            userState.IsAnonymous = answer == "Да";

            try
            {
                await MessageHandler.SendMessage(chatId, userState.Title, userState.Text, userState.IsAnonymous);
                
                AccountService accountService = SingletonService.GetAccountService();
                var user = await accountService.Read(chatId);

                var replyMarkup = user.Role switch
                {
                    Role.Graduate => Buttons.GraduateButtons(),
                    Role.Applicant => Buttons.ApplicantButtons(),
                    Role.Student => Buttons.StudentButtons(),
                    _ => null
                };

                await Client.SendTextMessageAsync(chatId, "Вы отправили сообщение", replyMarkup: replyMarkup);
            }
            catch (Exception ex)
            {
                // Обработка других исключений, если необходимо
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Очистка состояния пользователя после завершения
                userStates.TryRemove(chatId, out _);
            }
        }
    }
}