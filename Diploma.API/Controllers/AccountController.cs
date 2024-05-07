﻿using Diploma.API.Repositories;
using Diploma.API.Services;
using Diploma.Common.Models;
using Diploma.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace Diploma.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserRepository UserRepository;
    private readonly SessionService SessionService;

    public AccountController(UserRepository userRepository, SessionService sessionService)
    {
        UserRepository = userRepository;
        SessionService = sessionService;
    }
    
    [HttpGet("{chatId}")]
    public async Task<User?> Read(long chatId)
    {
        var user = await UserRepository.ReadFirst(u => u.ChatId == chatId);
        return user;
    }
    
    [HttpPost("Login")]
    public async Task<Session> Login(LoginModel loginModel)
    {
        var user = await UserRepository.ReadFirst(u =>
            u.Login == loginModel.Login && u.Password == loginModel.Password);
        if (user != null)
        {
            user.Token = Guid.NewGuid().ToString();
            var session = await SessionService.Create(user);
            return session;
        }
        else
        {
            return null;
        }
    }
    
    [HttpPost("Authenticate")]
    public async Task<Session> Authenticate()
    {
        try
        {
            var session = SessionService.ReadSession();
            if (session != null) return session;
            else return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    
    [HttpPost]
    public async Task Registration([FromBody] RegistrationModel model)
    {
        User user = new User
        {
            ChatId = model.ChatId,
            PhoneNumber = model.PhoneNumber,
            Name = model.Name,
            GroupId = model.GroupId
        };
        await UserRepository.Create(user);
    }
}