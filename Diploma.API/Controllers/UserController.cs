using Diploma.API.Repositories;
using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
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
        var users = (await UserRepository.Read(u => u.ChatId != null && (u.Role == Role.Student ||
                                                                         u.Role == Role.Graduate || u.Role == Role.Applicant),
            include: u => u.Group)).ToList();
        return users;
    }
    
    [HttpPost("WebUsers")]
    public async Task<List<User>> GetWebUsers()
    {
        var users = (await UserRepository.Read(u => u.Role == Role.Admin || u.Role == Role.Professor)).ToList();
        return users;
    }
}