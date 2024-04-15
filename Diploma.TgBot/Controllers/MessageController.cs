using Diploma.Common.Models;
using Diploma.Common.Services;
using Diploma.TgBot.Handlers;
using Diploma.TgBot.Services;
using Refit;
using Telegram.Bot;
using TgBotLib.Core;
using TgBotLib.Core.Base;

namespace Diploma.TgBot.Controllers;

public class MessageController : BotController
{
    private readonly IInlineButtonsGenerationService _buttonsGenerationService;
    private readonly IUsersActionsService _usersActionsService;

    private static string tittle;
    private static bool isAnonymous;
    
    public MessageController(IUsersActionsService usersActionsService, IInlineButtonsGenerationService buttonsGenerationService)
    {
        _usersActionsService = usersActionsService;
        _buttonsGenerationService = buttonsGenerationService;
    }
    
    [Callback("Отправить сообщение")]
    public async Task InitHandling()
    {
        _usersActionsService.HandleUser(BotContext.Update.GetChatId(), nameof(MessageController));
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), "Отправьте сообщение");
    }
    
    [ActionStep(nameof(MessageController), 0)]
    public async Task FirstStep()
    {
        tittle = Update.Message.Text;
        await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы хотите отправить сообщение анонимно?");
    }

    [ActionStep(nameof(MessageController), 1)]
    public async Task SecondStep()
    {
        try
        {
            var answer = Update.Message.Text;
            if (answer == "Да") isAnonymous = true;
            if (answer == "Нет") isAnonymous = false;

            MessagesService messagesService = SingletonService.GetMessagesService();
            AccountService accountService = SingletonService.GetAccountService();

            var user = await accountService.Read(BotContext.Update.GetChatId());
    
            Messages messages = new Messages
            {
                Tittle = tittle,
                DateTime = DateTime.UtcNow,
                IsAnonymous = isAnonymous,
                UserId = user.Id
            };

            // Добавьте обработку исключений для получения подробных сообщений об ошибках
            try
            {
                await messagesService.Create(messages);
            }
            catch (Refit.ValidationApiException ex)
            {
                var problemDetails = await ex.GetContentAsAsync<ProblemDetails>();

                // Печатаем подробности ошибки
                Console.WriteLine($"Validation error: {problemDetails.Title}");
                Console.WriteLine($"Status: {problemDetails.Status}");
                Console.WriteLine($"Detail: {problemDetails.Detail}");
            }

            await Client.SendTextMessageAsync(BotContext.Update.GetChatId(), $"Вы отправили сообщение");
        }
        catch (Exception ex)
        {
            // Обработка других исключений, если необходимо
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}