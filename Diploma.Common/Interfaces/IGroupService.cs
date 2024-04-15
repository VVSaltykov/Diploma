using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Diploma.Common.Interfaces;

public interface IGroupService
{
    [Post("/api/Group")]
    Task Create([FromBody] Group group);

    [Get("/api/Group/{name}")]
    Task<Group> Read(string name);
}