using Diploma.Common.Models;
using Diploma.Common.Services;
using Diploma.TgBot.Services;

namespace Diploma.TgBot.Handlers;

public static class RegistrationHandler
{
    public static async Task RegistrationUser(long chatId, string phoneNumber, string name, string groupName)
    {
        AccountService accountService = SingletonService.GetAccountService();
        GroupService groupService = SingletonService.GetGroupService();
        
        Group group = new Group();

        group = await groupService.Read(groupName);
        
        RegistrationModel registrationModel = new RegistrationModel
        {
            ChatId = chatId,
            PhoneNumber = phoneNumber,
            Name = name,
            GroupId = group.Id
        };

        await accountService.Registration(registrationModel);
    }
}