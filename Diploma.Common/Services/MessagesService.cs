using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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

    public async Task<List<Messages>> GetAll(List<User> TelegramUsers)
    {
        var messages = await _messagesService.GetAll(TelegramUsers);
        return messages;
    }

    public async Task<List<Messages>> GetPrivateMessages(Guid userId)
    {
        var messages = await _messagesService.GetPrivateMessages(userId);
        return messages;
    }

    public async Task<List<Messages>> GetMessagesForUser(long chatId)
    {
        var messages = await _messagesService.GetMessagesForUser(chatId);
        return messages;
    }

    public async Task<List<Messages>> GetUserMessages(long chatId)
    {
        var messages = await _messagesService.GetUserMessages(chatId);
        return messages;
    }
}