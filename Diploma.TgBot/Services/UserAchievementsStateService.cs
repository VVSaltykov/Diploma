using Diploma.TgBot.Data;

namespace Diploma.TgBot.Services;

public class UserAchievementsStateService
{
    private static readonly Dictionary<long, UserAchievementsState> UserAchievementsStates = new();

    public static void SetUserAchievementState(long chatId, UserAchievementsState state)
    {
        UserAchievementsStates[chatId] = state;
    }

    public static UserAchievementsState GetUserAchievementState(long chatId)
    {
        UserAchievementsStates.TryGetValue(chatId, out var state);
        return state;
    }
}