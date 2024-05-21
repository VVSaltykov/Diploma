﻿using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Diploma.Common.Interfaces;

public interface IMessagesService
{
    [Post("/api/Messages")]
    Task Create([FromBody] Messages messages);
    [Post("/api/Messages/GetAll")]
    Task<List<Messages>> GetAll(List<User> TelegramUsers);
}