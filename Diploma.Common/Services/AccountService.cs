using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.Common.Services;

public class AccountService : IAccountService
{
    private readonly IAccountService _accountService;
    
    public AccountService(HttpClient httpClient)
    {
        _accountService = RefitFunctions.GetRefitService<IAccountService>(httpClient);
    }

    public async Task<Session> Login(LoginModel loginModel)
    {
        try
        {
            var session = await _accountService.Login(loginModel);
            return session;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task Registration(RegistrationModel model)
    {
        await _accountService.Registration(model);
    }

    public async Task<User?> Read(long chatId)
    {
        var user = await _accountService.Read(chatId);
        return user;
    }

    public async Task<Session> Authenticate()
    {
        try
        {
            var session = await _accountService.Authenticate();
            return session;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Exception: {ex.Message}");
            throw;
        }
    }
}