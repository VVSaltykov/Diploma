using System.Text;
using Diploma.API.Data;
using Diploma.API.Repositories;
using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Diploma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly MessagesRepository _messagesRepository;
    private readonly UserRepository _userRepository;

    public MessagesController(MessagesRepository messagesRepository, UserRepository userRepository)
    {
        _messagesRepository = messagesRepository;
        _userRepository = userRepository;
    }
    
    [HttpPost]
    public async Task Create([FromBody] Messages messages)
    {
        try
        {
            Messages _messages = new Messages
            {
                Tittle = messages.Tittle,
                DateTime = DateTime.UtcNow,
                IsAnonymous = messages.IsAnonymous,
                UserId = messages.UserId,
                RecepientId = messages.RecepientId
            };
            await _messagesRepository.Create(_messages);
            
            var hubContext = HttpContext.RequestServices.GetRequiredService<IHubContext<MessageHub>>();
            await hubContext.Clients.All.SendAsync("ReceiveMessage", _messages);
        }
        catch (Exception ex) {}
    }
    
    [HttpPost("GetAll")]
    public async Task<List<Messages>> GetAll(List<User> TelegramUsers)
    {
        try
        {
            List<Messages> messages = new List<Messages>();
            foreach (var telegramUser in TelegramUsers)
            {
                var message = (await _messagesRepository.Read(m => m.UserId == telegramUser.Id, m => m.User)).ToList();
                messages.AddRange(message);
            }
            return messages;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    private async Task SendMessageToBot(Messages message)
    {
    }
}