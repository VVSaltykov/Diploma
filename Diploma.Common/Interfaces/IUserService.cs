using Diploma.Common.Models;
using Refit;

namespace Diploma.Common.Interfaces;

public interface IUserService
{
    [Post("/api/User/TelegramUsers")]
    Task<List<User>> GetTelegramUsers();
}