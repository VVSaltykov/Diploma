using Diploma.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Diploma.Common.Interfaces;

public interface IAccountService
{
    [Post("/api/Account/Login")]
    Task<Session> Login(LoginModel loginModel);
    [Post("/api/Account")]
    Task Registration([FromBody] RegistrationModel model);

    [Get("/api/Account/{chatId}")]
    Task<User?> Read(long chatId);
    [Post("/api/Account/Authenticate")]
    Task<Session> Authenticate();
}