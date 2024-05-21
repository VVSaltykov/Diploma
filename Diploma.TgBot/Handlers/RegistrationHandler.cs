using Diploma.Common.Models;
using Diploma.Common.Models.Enums;
using Diploma.Common.Services;
using Diploma.TgBot.Services;

namespace Diploma.TgBot.Handlers;

public static class RegistrationHandler
{
    public static async Task RegistrationUser(long chatId, string phoneNumber, string name, Role role, string groupName = null)
    {
        AccountService accountService = SingletonService.GetAccountService();

        if (groupName != null)
        {
            GroupService groupService = SingletonService.GetGroupService();
        
            Group group = new Group();

            group = await groupService.Read(groupName);
            
            RegistrationModel registrationModel = new RegistrationModel
            {
                ChatId = chatId,
                PhoneNumber = phoneNumber,
                Name = name,
                GroupId = group.Id,
                Role = role
            };
            await accountService.Registration(registrationModel);
        }
        else
        {
            RegistrationModel registrationModel = new RegistrationModel
            {
                ChatId = chatId,
                PhoneNumber = phoneNumber,
                Name = name,
                Role = role
            };
            await accountService.Registration(registrationModel);
        }
    }
}