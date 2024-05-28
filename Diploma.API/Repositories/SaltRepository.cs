using Diploma.Common.Models;

namespace Diploma.API.Repositories;

public class SaltRepository : BaseCRUDRepository<Salt, Guid>
{
    private readonly AppDbContext _appDbContext;
    
    public SaltRepository(AppDbContext dbContext) : base(dbContext)
    {
        _appDbContext = dbContext;
    }
}