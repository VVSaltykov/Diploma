using Diploma.Common.Models;
using Diploma.Common.Services;
using Diploma.TgBot.Services;

namespace Diploma.TgBot.Handlers;

public static class MessageHandler
{
    public static async Task SendMessage(long chatId, string tittle, bool isAnonymous)
    {
        MessagesService messagesService = SingletonService.GetMessagesService();
        AccountService accountService = SingletonService.GetAccountService();

        var user = await accountService.Read(chatId);
    
        Messages messages = new Messages
        {
            Tittle = tittle,
            IsAnonymous = isAnonymous,
            UserId = user.Id
        };
        await messagesService.Create(messages);
    }
}