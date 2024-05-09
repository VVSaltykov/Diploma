using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Utils;
using Telegram.Bot;
using TgBotLib.Core;
using TgBotLib.Core.Base;

namespace Diploma.Common.Services;

public class MessagesService : IMessagesService
{
    private readonly IMessagesService _messagesService;
    
    public MessagesService(HttpClient httpClient)
    {
        _messagesService = RefitFunctions.GetRefitService<IMessagesService>(httpClient);
    }

    public Task Create(Messages messages)
    {
        return _messagesService.Create(messages);
    }

    public async Task<List<Messages>> GetAll()
    {
        var messages = await _messagesService.GetAll();
        return messages;
    }
}