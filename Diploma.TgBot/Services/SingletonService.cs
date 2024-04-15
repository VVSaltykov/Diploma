using Diploma.Common.Services;

namespace Diploma.TgBot.Services;

public static class SingletonService
{
    private static AccountService accountService;
    private static GroupService groupService;
    private static MessagesService messagesService;

    public static AccountService GetAccountService() => accountService ??= new AccountService(CreateClient());
    public static GroupService GetGroupService() => groupService ??= new GroupService(CreateClient());
    public static MessagesService GetMessagesService() => messagesService ??= new MessagesService(CreateClient());

    public static HttpClient CreateClient()
    {
        return new HttpClient()
        {
            BaseAddress = new Uri(BotSettings.BackRoot)
        };
    }
}