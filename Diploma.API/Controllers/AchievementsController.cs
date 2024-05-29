using Diploma.API.Data;
using Diploma.API.Repositories;
using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Diploma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AchievementsController : ControllerBase
{
    private readonly AchievementsRepository AchievementsRepository;
    private readonly MessagesRepository MessagesRepository;
    private readonly UserRepository UserRepository;

    public AchievementsController(AchievementsRepository achievementsRepository, MessagesRepository messagesRepository,
        UserRepository userRepository)
    {
        AchievementsRepository = achievementsRepository;
        MessagesRepository = messagesRepository;
        UserRepository = userRepository;
    }
    
    [HttpPost("Create")]
    public async Task<Achievements> Create(Achievements achievement)
    {
        Achievements _achievement = new Achievements
        {
            Tittle = achievement.Tittle,
            Text = achievement.Text,
        };
        
        await AchievementsRepository.Create(_achievement);
        
        foreach (var user in achievement.Users)
        {
            _achievement.Users.Add(user);
        }

        await AchievementsRepository.Update(_achievement);

        var telegramUsers = (await UserRepository.Read(u => u.Role == Role.Student ||
                                                            u.Role == Role.Applicant ||
                                                            u.Role == Role.Graduate)).ToList();
        List<long> telegramUserChatIds = new List<long>();

        telegramUserChatIds.AddRange(telegramUsers.Where(user => user.ChatId.HasValue).Select(user => user.ChatId.Value));
        
        Messages _messages = new Messages
        {
            Tittle = _achievement.Tittle,
            Text = _achievement.Text,
            DateTime = DateTime.UtcNow,
            RecepientInTelegramIds = telegramUserChatIds.Cast<long?>().ToList(),
        };
            
        var hubContext = HttpContext.RequestServices.GetRequiredService<IHubContext<MessageHub>>();
        await hubContext.Clients.All.SendAsync("ReceiveMessage", _messages);
        
        return achievement;
    }
    
    [HttpPost("GetAll")]
    public async Task<List<Achievements>> GetAll()
    {
        var achievements = (await AchievementsRepository.Read(include: a => a.Users)).ToList();
        return achievements;
    }
    
    [HttpPost("GetUserAchievements")]
    public async Task<List<Achievements>> GetUserAchievements(User user)
    {
        var achievements = (await AchievementsRepository.Read(a => a.Users.Any(u => u.Id == user.Id), include: a => a.Users)).ToList();
        return achievements;
    }
}