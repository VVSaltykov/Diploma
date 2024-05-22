using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Utils;

namespace Diploma.Common.Services;

public class UserService : IUserService
{
    private readonly IUserService _userService;
    
    public UserService(HttpClient httpClient)
    {
        _userService = RefitFunctions.GetRefitService<IUserService>(httpClient);
    }
    
    public async Task<List<User>> GetTelegramUsers()
    {
        var users = await _userService.GetTelegramUsers();
        return users;
    }

    public async Task<List<User>> GetWebUsers()
    {
        var users = await _userService.GetWebUsers();
        return users;
    }

    public async Task<User> GetProfessor(string professorName)
    {
        return await _userService.GetProfessor(professorName);
    }

    public async Task<User> GetUserById(Guid userId)
    {
        return await _userService.GetUserById(userId);
    }
}