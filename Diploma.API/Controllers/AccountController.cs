using Diploma.API.Repositories;
using Diploma.API.Services;
using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserRepository UserRepository;
    private readonly GroupRepository GroupRepository;
    private readonly SessionService SessionService;
    private readonly SaltService _saltService;

    public AccountController(UserRepository userRepository, GroupRepository groupRepository, SessionService sessionService,
        SaltService saltService)
    {
        UserRepository = userRepository;
        GroupRepository = groupRepository;
        SessionService = sessionService;
        _saltService = saltService;
    }
    
    [HttpGet("{chatId}")]
    public async Task<User?> Read(long chatId)
    {
        var user = await UserRepository.ReadFirst(u => u.ChatId == chatId);
        return user;
    }
    
    [HttpPost("Login")]
    public async Task<Session> Login(LoginModel loginModel)
    {
        //var userPassword = await _saltService.GetHashData(loginModel.Password, login: loginModel.Login);
        var user = await UserRepository.ReadFirst(u =>
            u.Login == loginModel.Login && u.Password == loginModel.Password);
        if (user != null)
        {
            user.Token = Guid.NewGuid().ToString();
            var session = await SessionService.Create(user);
            return session;
        }
        else
        {
            return null;
        }
    }
    
    [HttpPost("Authenticate")]
    public async Task<Session> Authenticate()
    {
        try
        {
            var session = SessionService.ReadSession();
            if (session != null) return session;
            else return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    [HttpPost]
    public async Task Registration([FromBody] RegistrationModel model)
    {
        if (model.Role == Role.Applicant)
        {
            User user = new User
            {
                ChatId = model.ChatId,
                Name = model.Name,
                Role = model.Role
            };
            user.PhoneNumber = await _saltService.HashData(model.PhoneNumber, user);
            
            await UserRepository.Create(user);
        }
        else if (model.Role == Role.Admin || model.Role == Role.Professor)
        {
            User user = new User
            {
                Login = model.Login,
                Name = model.Name,
                Role = model.Role,
                Salt = new Salt()
            };
            user.Password = await _saltService.HashData(model.Password, user);
            //if ((await UserRepository.Read(u => u.Login == user.Login)).Any()) //TODO
            await UserRepository.Create(user);
        }
        else
        {
            var group = await GroupRepository.ReadFirst(g => g.Id == model.GroupId);
            User user = new User
            {
                ChatId = model.ChatId,
                PhoneNumber = model.PhoneNumber,
                Name = model.Name,
                GroupId = model.GroupId,
                Group = group,
                Role = model.Role,
                Salt = new Salt()
            };
            user.PhoneNumber = await _saltService.HashData(model.PhoneNumber, user);
            
            await UserRepository.Create(user);
        }
    }
}