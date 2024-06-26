﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Diploma.Common.Interfaces;

public interface IGroupService
{
    [Post("/api/Group")]
    Task Create([FromBody] Group group);

    [Post("/api/Group/{name}")]
    Task<Group> Read(string name);

    [Post("/api/Group/GetAll")]
    Task<List<Group>> GetAll();

    [Post("/api/Group/GetGroupById")]
    Task<Group> GetGroupById(int? groupId);
}