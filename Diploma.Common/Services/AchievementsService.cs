using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Utils;

namespace Diploma.Common.Services;

public class AchievementsService : IAchievementsService
{
    private readonly IAchievementsService _achievementsService;
    
    public AchievementsService(HttpClient httpClient)
    {
        _achievementsService = RefitFunctions.GetRefitService<IAchievementsService>(httpClient);
    }

    public async Task<Achievements> Create(Achievements achievement)
    {
        return await _achievementsService.Create(achievement);
    }

    public async Task<List<Achievements>> GetAll()
    {
        var achievements = await _achievementsService.GetAll();
        return achievements;
    }

    public async Task<List<Achievements>> GetUserAchievements(User user)
    {
        var achievements = await _achievementsService.GetUserAchievements(user);
        return achievements;
    }
}