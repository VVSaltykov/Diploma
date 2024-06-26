﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diploma.Common.Models;
using Refit;

namespace Diploma.Common.Interfaces;

public interface IUserService
{
    [Post("/api/User/TelegramUsers")]
    Task<List<User>> GetTelegramUsers();
    
    [Post("/api/User/WebUsers")]
    Task<List<User>> GetWebUsers();

    [Post("/api/User/Professor")]
    Task<User> GetProfessor(string professorName);

    [Post("/api/User/GetUserById")]
    Task<User> GetUserById(Guid userId);
}