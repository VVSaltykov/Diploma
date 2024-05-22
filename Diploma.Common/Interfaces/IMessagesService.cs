using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Diploma.Common.Interfaces;

public interface IMessagesService
{
    [Post("/api/Messages")]
    Task Create([FromBody] Messages messages);
    [Post("/api/Messages/GetAll")]
    Task<List<Messages>> GetAll(List<User> TelegramUsers);

    [Post("/api/Messages/GetPrivateMessages")]
    Task<List<Messages>> GetPrivateMessages(Guid userId);

    [Post("/api/Messages/GetMessagesForUser")]
    Task<List<Messages>> GetMessagesForUser(long chatId);

    [Post("/api/Messages/GetUserMessages")]
    Task<List<Messages>> GetUserMessages(long chatId);
}