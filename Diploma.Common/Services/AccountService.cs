using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Utils;

namespace Diploma.Common.Services;

public class AccountService : IAccountService
{
    private readonly IAccountService _accountService;
    
    public AccountService(HttpClient httpClient)
    {
        _accountService = RefitFunctions.GetRefitService<IAccountService>(httpClient);
    }

    public async Task Registration(RegistrationModel model)
    {
        await _accountService.Registration(model);
    }

    public async Task<User> Read(long chatId)
    {
        User result = default;
        result = await _accountService.Read(chatId);
        return result;
    }
}