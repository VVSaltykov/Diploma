using Diploma.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Diploma.API.Repositories;

public class GroupRepository : BaseCRUDRepository<Group, int>
{
    private readonly AppDbContext _appDbContext;
    
    public GroupRepository(AppDbContext dbContext) : base(dbContext)
    {
        _appDbContext = dbContext;
    }

    public async Task<Group> GetGroupByName(string groupName)
    {
        Group group = await _appDbContext.Groups.Where(g => g.Name == groupName).Include(g => g.Users).FirstOrDefaultAsync();
        if (group == null)
        {
            group = new Group
            {
                Name = groupName
            };
            await Create(group);
            return group;
        }
        return group;
    }

    public async Task AddUserToGroup(User user)
    {
        var group = await _appDbContext.Groups.Include(g => g.Users).Where(g => g.Id == user.GroupId)
            .FirstOrDefaultAsync();
        group.Users.Add(user);
        await _appDbContext.SaveChangesAsync();
    }
}