using Diploma.API.Repositories;
using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserRepository UserRepository;

    public AccountController(UserRepository userRepository)
    {
        UserRepository = userRepository;
    }
    
    [HttpPost("Registrate")]
    public async Task Registrate([FromForm] RegistrationModel model)
    {
        await UserRepository.Create(model.ChatId, model.PhoneNumber);
    }
}