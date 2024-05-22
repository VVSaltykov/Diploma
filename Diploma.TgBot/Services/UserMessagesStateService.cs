using Diploma.TgBot.Data;

namespace Diploma.TgBot.Services;

public static class UserMessagesStateService
{
    private static readonly Dictionary<long, UserMessagesState> userMessagesStates = new();

    public static void SetUserMessagesState(long chatId, UserMessagesState state)
    {
        userMessagesStates[chatId] = state;
    }

    public static UserMessagesState GetUserMessagesState(long chatId)
    {
        userMessagesStates.TryGetValue(chatId, out var state);
        return state;
    }
}