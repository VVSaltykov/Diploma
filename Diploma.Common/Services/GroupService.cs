using Diploma.Common.Interfaces;
using Diploma.Common.Models;
using Diploma.Common.Utils;

namespace Diploma.Common.Services;

public class GroupService : IGroupService
{
    private IGroupService _groupService;

    public GroupService(HttpClient httpClient)
    {
        _groupService = RefitFunctions.GetRefitService<IGroupService>(httpClient);
    }
    
    public Task Create(Group group)
    {
        return _groupService.Create(group);
    }

    public Task<Group> Read(string name)
    {
        return _groupService.Read(name);
    }
}