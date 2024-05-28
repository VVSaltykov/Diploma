using Diploma.Common.Models;

namespace Diploma.TgBot.Data;

public class UserAchievementsState
{
    public List<Achievements> Achievements { get; set; } = new List<Achievements>();
    public int CurrentAchievementIndex { get; set; } = 0;
    public int LastAchievementId { get; set; } = 0;
}