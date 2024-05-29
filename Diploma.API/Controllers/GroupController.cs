using Diploma.API.Repositories;
using Diploma.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace Diploma.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly GroupRepository _groupRepository;

    public GroupController(GroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }
    
    [HttpPost]
    public async Task Create([FromBody] Group model)
    {
        Group group = new Group
        {
            Name = model.Name
        };

        await _groupRepository.Create(group);
    }
    
    [HttpPost("GetAll")]
    public async Task<List<Group>> GetAll()
    {
        List<Group> groups = (await _groupRepository.Read(include: g => g.Users)).ToList();
        return groups;
    }
    
    [HttpPost("{name}")]
    public async Task<Group> Read(string name)
    {
        Group group = await _groupRepository.GetGroupByName(name);
        return group;
    }
    
    [HttpPost("GetGroupById")]
    public async Task<Group> GetGroupById(int? groupId)
    {
        Group group = await _groupRepository.ReadFirst(g => g.Id == groupId);
        return group;
    }
}