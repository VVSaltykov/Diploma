using Diploma.API.Repositories;
using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;

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
                DateTime = messages.DateTime,
                IsAnonymous = messages.IsAnonymous,
                UserId = messages.UserId
            };
            await _messagesRepository.Create(_messages);
        }
        catch (Exception ex) {}
    }
    
    [HttpPost("GetAll")]
    public async Task<List<Messages>> GetAll()
    {
        try
        {
            List<Messages> messages = (await _messagesRepository.Read(include: m => m.User)).ToList();
            return messages;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}