using Diploma.Common.Models;
using Diploma.Common.Services;
using Diploma.TgBot.Services;

namespace Diploma.TgBot.Handlers;

public static class MessageHandler
{
    // public static async Task SendMessage(long chatId, string tittle, bool isAnonymous)
    // {
    //     MessagesService messagesService = SingletonService.GetMessagesService();
    //     
    //     Messages messages = new Messages
    //     {
    //         Tittle = tittle,
    //         DateTime = DateTime.Now,
    //         IsAnonymous = isAnonymous
    //     };
    //
    //     await messagesService.Create(messages, chatId);
    // }
}