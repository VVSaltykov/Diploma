using System.Collections.Concurrent;
using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Diploma.Common.Utils;
using Diploma.TgBot.Data;
using Diploma.TgBot.Handlers;
using Diploma.TgBot.Services;
using Diploma.TgBot.UI;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
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

    [Message("Отправить сообщение", MessageType.Text)]
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

            await Client.SendTextMessageAsync(chatId, "Хотите ли вы прикрепить файлы к сообщению?", replyMarkup: new ReplyKeyboardMarkup(new[] { new KeyboardButton("Да"), new KeyboardButton("Нет") }) { ResizeKeyboard = true, OneTimeKeyboard = true });
        }
    }
    
    [ActionStep(nameof(MessageController), 3)]
    public async Task FourthStep()
    {
        long chatId = BotContext.Update.GetChatId();
        
        if (userStates.TryGetValue(chatId, out var userState))
        {
            var answer = Update.Message.Text;
            if (answer == "Да")
            {
                await Client.SendTextMessageAsync(chatId, "Пожалуйста, отправьте файл.");
            }
            else
            {
                await SendMessage(chatId);
                _usersActionsService.IncrementStep(chatId);
            }
        }
    }
    
    [ActionStep(nameof(MessageController), 4)]
    public async Task FifthStep()
    {
        IFilesService filesService = SingletonService.GetFilesService();
        long chatId = BotContext.Update.GetChatId();
        
        if (userStates.TryGetValue(chatId, out var userState))
        {
            if (Update.Message.Document != null)
            {
                string fileId = Update.Message.Document.FileId;
                userState.FileIds.Add(fileId);

                var fileStream = await FilesFunctions.GetFileStreamAsync(Client, fileId); 
                var fileData = await FilesFunctions.ConvertStreamToByteArrayAsync(fileStream); 

                Files file = new Files
                {
                    FileId = fileId,
                    FileName = Update.Message.Document.FileName,
                    FileData = fileData
                };

                await filesService.Create(file); 
                
                await Client.SendTextMessageAsync(chatId, "Файл получен. Вы хотите прикрепить еще файл?", replyMarkup: new ReplyKeyboardMarkup(new[] { new KeyboardButton("Да"), new KeyboardButton("Нет") }) { ResizeKeyboard = true, OneTimeKeyboard = true });
            }
            else
            {
                await Client.SendTextMessageAsync(chatId, "Пожалуйста, отправьте файл.");
            }
        }
    }
    
    [ActionStep(nameof(MessageController), 5)]
    public async Task SixStep()
    {
        long chatId = BotContext.Update.GetChatId();
        
        if (userStates.TryGetValue(chatId, out var userState))
        {
            var answer = Update.Message.Text;
            if (answer == "Да")
            {
                await Client.SendTextMessageAsync(chatId, "Пожалуйста, отправьте файл");
                _usersActionsService.DecrementStep(chatId);
            }
            else
            {
                await SendMessage(chatId);
            }
        }
    }
    
    
    private async Task SendMessage(long chatId)
    {
        if (userStates.TryGetValue(chatId, out var userState))
        {
            try
            {
                await MessageHandler.SendMessage(chatId, userState.Title, userState.Text, userState.IsAnonymous, userState.FileIds);
                
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
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                userStates.TryRemove(chatId, out _);
            }
        }
    }
}