using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diploma.Common.Models;
using Refit;

namespace Diploma.Common.Interfaces;

public interface IAchievementsService
{
    [Post("/api/Achievements/Create")]
    Task<Achievements> Create(Achievements achievement);

    [Post("/api/Achievements/GetAll")]
    Task<List<Achievements>> GetAll();

    [Post("/api/Achievements/GetUserAchievements")]
    Task<List<Achievements>> GetUserAchievements(User user);
}