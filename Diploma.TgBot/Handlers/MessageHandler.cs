using Diploma.Common.Models;
using Diploma.Common.Services;
using Diploma.TgBot.Services;

namespace Diploma.TgBot.Handlers;

public static class MessageHandler
{
    public static async Task SendMessage(long chatId, string tittle, string text, bool isAnonymous, List<string> fileIds)
    {
        MessagesService messagesService = SingletonService.GetMessagesService();
        AccountService accountService = SingletonService.GetAccountService();
        
        var user = await accountService.Read(chatId);
        
        Messages messages = new Messages
        {
            Tittle = tittle,
            Text = text,
            IsAnonymous = isAnonymous,
            UserId = user.Id,
            FilesIds = fileIds
        };
        await messagesService.Create(messages);
    }
    public static async Task SendToProfessorMessage(long chatId, string tittle, string text, string professorName, List<string> fileIds)
    {
        MessagesService messagesService = SingletonService.GetMessagesService();
        AccountService accountService = SingletonService.GetAccountService();
        UserService userService = SingletonService.GetUserService();

        var user = await accountService.Read(chatId);
        var professor = await userService.GetProfessor(professorName);
    
        Messages messages = new Messages
        {
            Tittle = tittle,
            Text = text,
            UserId = user.Id,
            FilesIds = fileIds
        };
        messages.RecepientInWebIds.Add(professor.Id);
        await messagesService.Create(messages);
    }
}