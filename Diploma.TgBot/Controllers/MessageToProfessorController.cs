using System.Collections.Concurrent;
using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
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

public class MessageToProfessorController : BotController
{
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IUsersActionsService _usersActionsService;
    
    private static string tittle;
    private static string text;
    private static string professorName;
    
    private static ConcurrentDictionary<long, UserSentMessageState> userStates = new ConcurrentDictionary<long, UserSentMessageState>();
    
    public MessageToProfessorController(IUsersActionsService usersActionsService, IInlineButtonsGenerationService buttonsGenerationService)
    {
        _usersActionsService = usersActionsService;
        _buttonsGenerationService = buttonsGenerationService;
    }
    
    [Message("Отправить сообщение преподавателю", MessageType.Text)]
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

            await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), "Хотите ли вы прикрепить файлы к сообщению?", replyMarkup: new ReplyKeyboardMarkup(new[] { new KeyboardButton("Да"), new KeyboardButton("Нет") }) { ResizeKeyboard = true, OneTimeKeyboard = true });
        }
        catch (Exception ex)
        {
            // Обработка других исключений, если необходимо
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    
    [ActionStep(nameof(MessageToProfessorController), 3)]
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
    
    [ActionStep(nameof(MessageToProfessorController), 4)]
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

                var fileStream = await GetFileStreamAsync(fileId); // Получаем поток файла
                var fileData = await ConvertStreamToByteArrayAsync(fileStream); // Конвертируем поток в байты

                Files file = new Files
                {
                    FileId = fileId,
                    FileName = Update.Message.Document.FileName,
                    FileData = fileData
                };

                await filesService.Create(file); // Сохраняем файл в базу данных
                
                await Client.SendTextMessageAsync(chatId, "Файл получен. Вы хотите прикрепить еще файл?", replyMarkup: new ReplyKeyboardMarkup(new[] { new KeyboardButton("Да"), new KeyboardButton("Нет") }) { ResizeKeyboard = true, OneTimeKeyboard = true });
            }
            else
            {
                await Client.SendTextMessageAsync(chatId, "Пожалуйста, отправьте файл.");
            }
        }
    }
    
    [ActionStep(nameof(MessageToProfessorController), 5)]
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
                await MessageHandler.SendToProfessorMessage(chatId, userState.Title, userState.Text, userState.ProfessorName, userState.FileIds);
                
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
    
    private async Task<Stream> GetFileStreamAsync(string fileId)
    {
        var file = await Client.GetFileAsync(fileId);
        var memoryStream = new MemoryStream();
        await Client.DownloadFileAsync(file.FilePath, memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin); // Сбрасываем указатель потока в начало
        return memoryStream;
    }

    // Метод для конвертации потока в массив байтов
    private async Task<byte[]> ConvertStreamToByteArrayAsync(Stream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}