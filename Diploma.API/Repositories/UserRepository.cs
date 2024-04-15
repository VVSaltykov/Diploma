using Diploma.Common.Models;

namespace Diploma.API.Repositories;

public class UserRepository : BaseCRUDRepository<User, Guid>
{
    private readonly AppDbContext _appDbContext;
    
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
        _appDbContext = dbContext;
    }
}