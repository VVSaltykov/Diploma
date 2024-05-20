using Diploma.API.Repositories;
using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserRepository UserRepository;

    public UserController(UserRepository userRepository)
    {
        UserRepository = userRepository;
    }
    
    [HttpPost("TelegramUsers")]
    public async Task<List<User>> GetTelegramUsers()
    {
        var users = (await UserRepository.Read(u => u.ChatId != null)).ToList();
        return users;
    }
}