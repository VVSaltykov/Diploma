using Diploma.API.Repositories;
using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserRepository UserRepository;
    private readonly GroupRepository GroupRepository;

    public AccountController(UserRepository userRepository, GroupRepository groupRepository)
    {
        UserRepository = userRepository;
        GroupRepository = groupRepository;
    }
    
    [HttpGet("{chatId}")]
    public async Task<User?> Read(long chatId)
    {
        var user = await UserRepository.ReadFirst(u => u.ChatId == chatId);
        return user;
    }
    
    [HttpPost("Login")]
    public async Task<User> Login(LoginModel loginModel)
    {
        var user = await UserRepository.ReadFirst(u =>
            u.Login == loginModel.Login && u.Password == loginModel.Password);
        return user;
    }
    
    [HttpPost]
    public async Task Registration([FromBody] RegistrationModel model)
    {
        User user = new User
        {
            ChatId = model.ChatId,
            PhoneNumber = model.PhoneNumber,
            Name = model.Name,
            GroupId = model.GroupId
        };
        await UserRepository.Create(user);
    }
}